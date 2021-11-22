﻿/*
 * Copyright, 2021, LogicMonitor, Inc.
 * This Source Code Form is subject to the terms of the 
 * Mozilla Public License, v. 2.0. If a copy of the MPL 
 * was not distributed with this file, You can obtain 
 * one at https://mozilla.org/MPL/2.0/.
 */

using NUnit.Framework;
using LogicMonitor.DataSDK;
using RestSharp;
using System;
using LogicMonitor.DataSDK.Model;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace LogicMonitor.DataSDK.Tests
{
    [TestFixture]
    public class TestApiClients
    {
        Configuration configuration;
        ApiClient apiClients;


        [SetUp]
        public void Setup()
        {
            Authenticate authenticate = new Authenticate();
            authenticate.Id = "xz5i6L7Pz44k4wAb4b5r";
            authenticate.Key = "F972B{jWL-JI+Z}M=(aA~~=fcD(y[^993]pCyjS+";
            authenticate.Type = "LMv1";
            configuration = new Configuration(company: "lmaakashkhopade", authentication: authenticate);
            apiClients = new ApiClient(configuration);
        }

        [Test]
        public void TestHeaderContentType()
        {
            string content = "application/json";
            Assert.AreEqual(content, apiClients.SelectHeaderAccept("application/json"));
        }
        [Test]
        public void TestHeaderContentTypeEmpty()
        {
            string content = "application/json";
            Assert.AreEqual(content, apiClients.SelectHeaderContentType(""));
        }

        [Test]
        public void TestHeaderAccept()
        {
            string content = "application/json";
            Assert.AreEqual(content, apiClients.SelectHeaderAccept("application/json"));
        }
        [Test]
        public void TestHeaderAcceptEmpty()
        {
            string content = "application/json";
            Assert.AreEqual(content, apiClients.SelectHeaderContentType(""));
        }

        

        [Test]
        public void TestCallapi()
        {

            Dictionary<string, string> pathParams = new Dictionary<string, string>();
            var queryParams = new Dictionary<string, string>();

            var headersParams = new Dictionary<String, string>();
            headersParams.Add("Accept", "application/json");
            headersParams.Add("Content-Type", "application/json");
            var a = apiClients.Callapi(path: "/metric/ingest", method: "POST",_request_timeout: TimeSpan.FromMinutes(2),queryParams:queryParams,headerParams:headersParams);
            //Console.WriteLine(a.Content.ToString());
            Assert.Pass();
        }

        [TestCase("ABCD1234","body{}")]
        public void TestHmacSHA256(string key, string data)
        {
            string expected = "097adf7cbbe4be168eabbf41032e1ae163bb709b7d45c4ae256fa66065367c49";
            string actual = ApiClient.HmacSHA256(key, data);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("ABCD1234", "body{}")]
        public void TestToHexString(string key, string data)
        {
            string expected = "097adf7cbbe4be168eabbf41032e1ae163bb709b7d45c4ae256fa66065367c49";
            string actual;
            ASCIIEncoding encoder = new ASCIIEncoding();
            var code = System.Text.Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(code))
            {
                Byte[] hmBytes = hmac.ComputeHash(encoder.GetBytes(data));
                actual = ApiClient.ToHexString(hmBytes);

            }
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestUpdate_params_for_auth()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("ads", "sdsa");
            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("ads", "sdsa");
            Console.WriteLine(header.Count);
            var authcheck = apiClients.Update_params_for_auth(method: "POST", querys: query, auth_settings: "LMv1", resource_path: "/metric/ingest", body: "body{}", headers: header);
            Assert.AreEqual(true, authcheck);
        }
    }
}
