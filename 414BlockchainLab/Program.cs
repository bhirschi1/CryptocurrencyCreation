using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;


namespace _414BlockchainLab
{
    class Program
    {
        static void Main(string[] args)
        {
            // this built in method creates a private/public key pair
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();


            // 2 is the difficulty and 100 is the reward -> see the Blockchain class
            // changing difficulty to longer will mean looking for longer string of 0's, harder to find and takes longer to run
            // bitcoin adjusts the difficulty based on different factors to speed up or slow down how long
            // it takes to mine a block
            Blockchain bencoin = new Blockchain(3, 100);


            //normally first wave of crypto is added differently but for this we're just doing it this way
            Console.WriteLine("Start the Miner.");
            
            // after this, wallet1 should have 100 currency units and then we'll send to another one after 
            // checking balance making sure money got there
            bencoin.MinePendingTransactions(wallet1);
            Console.WriteLine("\nBalance of wallet1 is $" + bencoin.GetBalanceOfWallet(wallet1).ToString());

            // Create new transaction, sending money from wallet 1 to wallet 2
            // wallet1 should have 100 bencoin at the time of the transaction,
            // then send 10 to wallet2, 
            Transaction tx1 = new Transaction(wallet1, wallet2, 10);

            //now sign the transaction:
            tx1.SignTransaction(key1);
            //once found need to add to pending transactions
            bencoin.addPendingTransaction(tx1);
            Console.WriteLine("Start the miner.");
            bencoin.MinePendingTransactions(wallet2);
            Console.WriteLine("\nBalance of wallet1 is $" + bencoin.GetBalanceOfWallet(wallet1).ToString());
            Console.WriteLine("\nBalance of wallet2 is $" + bencoin.GetBalanceOfWallet(wallet2).ToString());

            string blockJSON = JsonConvert.SerializeObject(bencoin, Formatting.Indented);
            Console.WriteLine(blockJSON);

            //testing if the blockchain is invalid
            //bencoin.GetLatestBlock().PreviousHash = "1234";

            if (bencoin.IsChainValid())
            {
                Console.WriteLine("Blockchain is Valid!");
            }
            else
            {
                Console.WriteLine("Blockchain is NOT valid!");
            }
        }
    }

    

 
}
