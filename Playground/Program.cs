using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;

namespace NetCoreUniversity.Playground {

    public class Program {

        public static void Main(string[] args)
        {
            var teacher = JsonConvert.DeserializeObject<Teacher>("{\"menu\": {" +
                                                                 "\"id\": \"file\"," +
                                                                 "\"value\": \"File\"," +
                                                                 "\"popup\": {" +
                                                                 "\"menuitem\": [" +
                                                                 "{\"value\": \"New\", \"onclick\": \"CreateNewDoc()\"}," +
                                                                 "{\"value\": \"Open\", \"onclick\": \"OpenDoc()\"}," +
                                                                 "{\"value\": \"Close\", \"onclick\": \"CloseDoc()\"}" +
                                                                 "]}" +
                                                                 "}}");

            Console.ReadLine();
        }

    }
}