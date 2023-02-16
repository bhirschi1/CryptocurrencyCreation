using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace _414BlockchainLab
{
    class Block
    {
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public string Timestamp { get; set; }
        // public string Data { get; set; } This was just a placeholder before created the Transactions
        public string Hash { get; set; }
        public int Nonce { get; set; }
        public List<Transaction> Transactions { get; set; }

        //Constructor
        public Block(int index, string timestamp, List<Transaction> transactions, string previousHash = "")
        {
            this.Index = index;
            this.Timestamp = timestamp;
            this.Transactions = transactions;
            this.PreviousHash = previousHash;
            this.Hash = CalculateHash();
            this.Nonce = 0;
        }
        public string CalculateHash()
        {

            // this block validation is too simplistic to be realistic. 
            // instead can calculate merkle root which is what bitcoin does
            // must include the nonce at end of blockData 
            string blockData = this.Index + this.PreviousHash + this.Timestamp + this.Transactions.ToString() + this.Nonce;
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashBytes = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public void Mine(int difficulty)
        {
            // check to see if x number (based on difficulty) is = to a string of 0's w len(difficulty)
            while (this.Hash.Substring(0, difficulty) != new String('0', difficulty))
            {
                // Nonce is way of connecting blocks in blockchain, of necessity is difficult to do 
                // so you can't change or alter it
                this.Nonce++;
                //real world don't start at 0, start w a different isolated pool
                this.Hash = this.CalculateHash();
                //Just to visualize to see what hashes we're trying
                //Console.WriteLine("Mining: " + this.Hash);
            }

            Console.WriteLine("Block has been mined: " + this.Hash);
        }
    }
}
