//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

//Add the Facebook Unity SDK and uncomment this to enable Facebook login
//#define USE_FACEBOOK_LOGIN

using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoSync;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentity.Model;
using Amazon.CognitoSync.SyncManager;
/* 
namespace AWSSDK.Examples
{

    */ 
    public class Leaderboard : MonoBehaviour
    {

        private Dataset playerInfo;

        private string highScore, userName;

		private string statusMessage = "";

        public string IdentityPoolId = "us-east-1:e2024db9-bf94-485c-a95e-cf5ca6a321e3";
       
        public string Region = RegionEndpoint.USEast1.SystemName;
        
        private RegionEndpoint _Region
        {
            get { return RegionEndpoint.GetBySystemName(Region); }
        }

        private CognitoAWSCredentials _credentials;

        private CognitoAWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
					_credentials = new CognitoAWSCredentials(IdentityPoolId, _Region);
                    Debug.Log("HERE???");
                return _credentials;
            }
        }


        private CognitoSyncManager _syncManager;

        private CognitoSyncManager SyncManager
        {
            get
            {
                if (_syncManager == null)
                {
                    _syncManager = new CognitoSyncManager(Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = _Region });
                }
                return _syncManager;
            }
        }

        void Start()
        {

            UnityInitializer.AttachToGameObject(this.gameObject);

            // Open your datasets
            playerInfo = SyncManager.OpenOrCreateDataset("PlayerInfo");
            Debug.Log("IS IT BROKEN");

            // Fetch locally stored data from a previous run
            userName = "TESTINGNAME";
            highScore = "20";
            Debug.Log("IS It even here lol");
            //userName = string.IsNullOrEmpty(playerInfo.Get("userName")) ? "Enter your username" : playerInfo.Get("userName");
            //highScore = string.IsNullOrEmpty(playerInfo.Get("highScore")) ? "Enter your highscore" : playerInfo.Get("highScore");

			// Define Synchronize callbacks
			// when ds.SynchronizeAsync() is called the localDataset is merged with the remoteDataset 
            // OnDatasetDeleted, OnDatasetMerged, OnDatasetSuccess,  the corresponding callback is fired.
            // The developer has the freedom of handling these events needed for the Dataset
            playerInfo.OnSyncSuccess += this.HandleSyncSuccess; // OnSyncSucess uses events/delegates pattern
            playerInfo.OnSyncFailure += this.HandleSyncFailure; // OnSyncFailure uses events/delegates pattern
            playerInfo.OnSyncConflict = this.HandleSyncConflict;
            playerInfo.OnDatasetMerged = this.HandleDatasetMerged;
            playerInfo.OnDatasetDeleted = this.HandleDatasetDeleted;

            playerInfo.Put("userName", userName);
            playerInfo.Put("highScore", highScore);
            playerInfo.SynchronizeAsync();

        }

        /* public void ButtonClicked()
        {
            // Get the text from the separate text area
            string text = newName.text;

            Debug.Log("Data received: " + text);
            // Do something with the received data
        }
 */

        void OnGUI()
        {
            GUI.color = Color.gray;
            GUILayout.BeginArea(new Rect(Screen.width * 0.2f, 0, Screen.width - Screen.width * 0.2f, Screen.height));
            GUILayout.Space(20);
            GUILayout.Label(statusMessage);

            if (SyncManager == null)
            {
                GUILayout.EndArea();
                return;
            }

            GUI.color = Color.white;

            GUILayout.Label("Enter some text");
            highScore = GUILayout.TextField(highScore, GUILayout.MinHeight(20), GUILayout.Width(Screen.width * 0.6f));
            userName = GUILayout.TextField(userName, GUILayout.MinHeight(20), GUILayout.Width(Screen.width * 0.6f));

            GUILayout.Space(20);

            if (GUILayout.Button("Save offline", GUILayout.MinHeight(20), GUILayout.Width(Screen.width * 0.6f)))
            {
                statusMessage = "Saving offline";

                playerInfo.Put("highScore", highScore);
                playerInfo.Put("userName", userName);

				userName = string.IsNullOrEmpty(playerInfo.Get("userName")) ? "Enter your username" : playerInfo.Get("userName");
				highScore = string.IsNullOrEmpty(playerInfo.Get("highScore")) ? "Enter your name" : playerInfo.Get("highScore");

                statusMessage = "Saved offline";
            }
            else if (GUILayout.Button("Sync with Amazon Cognito", GUILayout.MinHeight(20), GUILayout.Width(Screen.width * 0.6f)))
            {
                statusMessage = "Saving to CognitoSync Cloud";
                playerInfo.Put("userName", userName);
				playerInfo.Put("highScore", highScore);
				playerInfo.SynchronizeAsync();
            }
            else if (GUILayout.Button("Delete local data", GUILayout.MinHeight(20), GUILayout.Width(Screen.width * 0.6f)))
            {
                statusMessage = "Deleting all local data";
                SyncManager.WipeData(false);
				highScore = "Enter your name";
				userName = "Enter your userName";
                statusMessage = "Deleting all local data complete. ";
            }
			GUILayout.Space(20);

        }

        #region Sync events
        private bool HandleDatasetDeleted(Dataset dataset)
        {

            statusMessage = dataset.Metadata.DatasetName + "Dataset has been deleted.";
            Debug.Log(dataset.Metadata.DatasetName + " Dataset has been deleted");

            // Clean up if necessary 

            // returning true informs the corresponding dataset can be purged in the local storage and return false retains the local dataset
            return true;
        }

        public bool HandleDatasetMerged(Dataset dataset, List<string> datasetNames)
        {
            statusMessage = "Merging datasets between different identities.";
            Debug.Log(dataset + " Dataset needs merge");
            // returning true allows the Synchronize to resume and false cancels it
            return true;
        }

        private bool HandleSyncConflict(Amazon.CognitoSync.SyncManager.Dataset dataset, List<SyncConflict> conflicts)
        {

            statusMessage = "Handling Sync Conflicts.";
            Debug.Log("OnSyncConflict");
            List<Amazon.CognitoSync.SyncManager.Record> resolvedRecords = new List<Amazon.CognitoSync.SyncManager.Record>();

            foreach (SyncConflict conflictRecord in conflicts)
            {
                // This example resolves all the conflicts using ResolveWithRemoteRecord 
                // SyncManager provides the following default conflict resolution methods:
                //      ResolveWithRemoteRecord - overwrites the local with remote records
                //      ResolveWithLocalRecord - overwrites the remote with local records
                //      ResolveWithValue - for developer logic  
                resolvedRecords.Add(conflictRecord.ResolveWithRemoteRecord());
            }

            // resolves the conflicts in local storage
            dataset.Resolve(resolvedRecords);

            // on return true the synchronize operation continues where it left,
            //      returning false cancels the synchronize operation
            return true;
        }

        private void HandleSyncSuccess(object sender, SyncSuccessEventArgs e)
        {

            var dataset = sender as Dataset;

			if (dataset.Metadata != null) {
            	Debug.Log("Successfully synced for dataset: " + dataset.Metadata);
			} else {
				Debug.Log("Successfully synced for dataset");
			}

            if (dataset == playerInfo)
            {
                userName = string.IsNullOrEmpty(playerInfo.Get("userName")) ? "Enter your userName" : dataset.Get("userName");
                highScore = string.IsNullOrEmpty(playerInfo.Get("highScore")) ? "Enter your name" : dataset.Get("highScore");
            }
            statusMessage = "Syncing to CognitoSync Cloud succeeded";
        }

        private void HandleSyncFailure(object sender, SyncFailureEventArgs e)
        {
            var dataset = sender as Dataset;
            Debug.Log("Sync failed for dataset : " + dataset.Metadata.DatasetName);
            Debug.LogException(e.Exception);

            statusMessage = "Syncing to CognitoSync Cloud failed";
        }
        #endregion
    }
//}

