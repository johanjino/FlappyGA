using UnityEngine;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using TMPro;



public class Leaderboard : MonoBehaviour{

    public TMP_InputField userNameInput;
    private AmazonDynamoDBClient client;
    private DynamoDBContext Context;

    void Start()
    {
        // Initialize the Amazon DynamoDB client
        Debug.Log("AAAAAAAAAAA");
        UnityInitializer.AttachToGameObject(this.gameObject);
        Debug.Log("LOLZIES");
        try
        {
            CognitoAWSCredentials credentials = new CognitoAWSCredentials("us-east-1:9f9f53aa-e0c2-48aa-a3e7-e3ee0a519d74", RegionEndpoint.USEast1);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to initialize DynamoDB client: " + ex.Message);
        }
        
        // Set up the Amazon DynamoDB client
        Debug.Log("BBBBBBBBBB");
        //client = new AmazonDynamoDBClient(credentials);
        Debug.Log("CCCCCCCCC");
        //Context = new DynamoDBContext(client);
        Debug.Log("INITIALISED???");
    }

    public void SaveScore(string username, int score)
    {
        // Create a new leaderboard item
        var item = new LeaderboardItem
        {
            Username = userNameInput.text,
            Score = score
        };

        // Save the item to the database
        Context.SaveAsync(item, (result) =>
        {
            if (result.Exception != null)
            {
                Debug.LogError(result.Exception);
            }
            else
            {
                Debug.Log("Score saved successfully");
            }
        });

    }


    
    public void getLeaderboard(int limit, System.Action<List<LeaderboardItem>> callback)
    {
        // Set up the query request
        var request = new QueryRequest
        {
            TableName = "TestingTable",
            KeyConditionExpression = "LeaderboardId = :lbid",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                {":lbid", new AttributeValue { S = "1" }}
            },
            Limit = limit,
            ScanIndexForward = false // Sort the results in descending order
        };

        // Query the database
        client.QueryAsync(request, (result) =>
        {
            if (result.Exception != null)
            {
                Debug.LogError(result.Exception);
                callback(null);
            }
            else
            {
                // Convert the results to leaderboard items
                List<LeaderboardItem> items = new List<LeaderboardItem>();
                foreach (var item in result.Response.Items)
                {
                    var leaderboardItem = new LeaderboardItem();
                    leaderboardItem.FromDocument(item);
                    items.Add(leaderboardItem);
                }


                


                // Invoke the callback with the leaderboard items
                callback(items);
            }
        });
    }
}


    [DynamoDBTable("TestingTable")]
    public class LeaderboardItem
    {
        [DynamoDBHashKey]
        public string Username { get; set; }

        [DynamoDBProperty]
        public int Score { get; set; }

        [DynamoDBIgnore]
        public int Rank { get; set; }

        public void FromDocument(Dictionary<string, AttributeValue> documentAttributes)
        {
            var document = Document.FromAttributeMap(documentAttributes);
            Username = document["Username"];
            Score = int.Parse(document["Score"]);
            Rank = int.Parse(document["Rank"]);
        }


        public Document ToDocument()
        {
            var document = new Document();
            document["Username"] = Username;
            document["Score"] = Score.ToString();
            document["Rank"] = Rank.ToString();
            return document;
        }
    }


    /* [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;


    public void getLeaderboard(){

        /*
        This is how to update leaderboard:

        for (int i = 0; i<names.Count; i++){
            names[i].text = //name;
            scores[i].text = //score.ToString (if int)
        }


        */
    //}

    //public void setleaderboardentry(string username, int score){
        //upload to server new username and score
    //}




