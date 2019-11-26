using System;
using System.IO;
using Amazon;
using Amazon.QLDB;
using Amazon.QLDBSession;
using Amazon.Runtime;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            AmazonQLDBSessionConfig config = new AmazonQLDBSessionConfig();

            string accessKey = "FILL_YOURS";
            string secretKey = "FILL_YOURS";

            var client = new Amazon.QLDBSession.AmazonQLDBSessionClient(accessKey, secretKey, RegionEndpoint.USEast2);

            var req = new Amazon.QLDBSession.Model.SendCommandRequest() { };

            req.StartSession = new Amazon.QLDBSession.Model.StartSessionRequest();
            req.StartSession.LedgerName = "ApexQA";
            var ssResult = client.SendCommandAsync(req).Result;
            var sto = ssResult.StartSession.SessionToken;
            
            req = new Amazon.QLDBSession.Model.SendCommandRequest() { };
            req.SessionToken = sto;
            req.StartTransaction = new Amazon.QLDBSession.Model.StartTransactionRequest();
            ssResult = client.SendCommandAsync(req).Result;
            var txId = ssResult.StartTransaction.TransactionId;
            //req.StartTransaction

            req = new Amazon.QLDBSession.Model.SendCommandRequest() { };
            req.SessionToken = sto;

            req.ExecuteStatement = new Amazon.QLDBSession.Model.ExecuteStatementRequest();
            req.ExecuteStatement.TransactionId = txId;

            req.ExecuteStatement.Statement = "Insert INTO AuditLogs { \'AuditId\' :  2 }";
            ssResult = client.SendCommandAsync(req).Result;

            req = new Amazon.QLDBSession.Model.SendCommandRequest() { };
            req.SessionToken = sto;
            req.CommitTransaction = new Amazon.QLDBSession.Model.CommitTransactionRequest();
            req.CommitTransaction.TransactionId = txId;

            //Not sure what to fill in the Commit Digest
            req.CommitTransaction.CommitDigest = new MemoryStream();
            
            //This errors out 
            ssResult = client.SendCommandAsync(req).Result;
        }
    }
}
