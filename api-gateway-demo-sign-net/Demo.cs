using aliyun_api_gateway_sdk.Constant;
using aliyun_api_gateway_sdk.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aliyun_api_gateway_sdk
{
    class Demo
    {
        private const String appKey = "appKey";
        private const String appSecret = "appSecret";
        private const String host = "http://test.alicloudapi.com";        

        static void Main(string[] args)
        {
            doGet();

            doPostForm();
            doPostStream();
            doPostString();

            doPutStream();
            doPutString();

            doDelete();

            doHead();
            Console.Read();

        }

        private static void doGet() {
            String path = "/get";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();            
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_TEXT);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_TEXT);
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

        
            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpGet(host, path, appKey, appSecret, 30000, headers, querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostForm() {
            String path = "/postform";          

            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_FORM);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");
                        
            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            bodys.Add("b-body2", "bodyvalue1");
            bodys.Add("a-body1", "bodyvalue2");

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPost(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostStream()
        {
            String path = "/poststream";
           
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();
            byte[] bobyContent = new byte[10];

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(bobyContent));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分
            bodys.Add("", BitConverter.ToString(bobyContent));

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPost(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostString()
        {
            String bobyContent = "{\"inputs\": [{\"image\": {\"dataType\": 50,\"dataValue\": \"base64_image_string(此行)\"},\"configure\": {\"dataType\": 50,\"dataValue\": \"{\"side\":\"face(#此行此行)\"}\"}}]}";

            String path = "/poststring";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(bobyContent)));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分
            bodys.Add("", bobyContent);

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPost(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }
        
        private static void doPutStream()
        {
            String path = "/putstream";
           
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();
            byte[] bobyContent = new byte[10];

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(bobyContent));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分
            bodys.Add("", BitConverter.ToString(bobyContent));

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPut(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPutString()
        {
            String bobyContent = "{\"inputs\": [{\"image\": {\"dataType\": 50,\"dataValue\": \"base64_image_string(此行)\"},\"configure\": {\"dataType\": 50,\"dataValue\": \"{\"side\":\"face(#此行此行)\"}\"}}]}";

            String path = "/putstring";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            Dictionary<String, String> bodys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            //注意：如果有非Form形式数据(body中只有value，没有key)；如果body中是key/value形式数据，不要指定此行
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(bobyContent)));
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //注意：业务body部分
            bodys.Add("", bobyContent);

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpPut(host, path, appKey, appSecret, 30000, headers, querys, bodys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doDelete()
        {
            String path = "/delete";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpDelete(host, path, appKey, appSecret, 30000, headers, querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doHead()
        {
            String path = "/head";

            Dictionary<String, String> headers = new Dictionary<string, string>();
            Dictionary<String, String> querys = new Dictionary<string, string>();
            List<String> signHeader = new List<String>();

            //设定Content-Type，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_JSON);
            //设定Accept，根据服务器端接受的值来设置
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            //如果是调用测试环境请设置
            //headers.Add(SystemHeader.X_CA_STAGE, "TEST");

            //注意：业务header部分，如果没有则无此行(如果有中文，请做Utf8ToIso88591处理)
            headers.Add("b-header2", MessageDigestUtil.Utf8ToIso88591("headervalue1"));
            headers.Add("a-header1", MessageDigestUtil.Utf8ToIso88591("headervalue2处理"));

            //注意：业务query部分，如果没有则无此行；请不要、不要、不要做UrlEncode处理
            querys.Add("b-query2", "queryvalue2");
            querys.Add("a-query1", "queryvalue1");

            //指定参与签名的header            
            signHeader.Add(SystemHeader.X_CA_TIMESTAMP);
            signHeader.Add("a-header1");
            signHeader.Add("b-header2");

            using (HttpWebResponse response = HttpUtil.HttpHead(host, path, appKey, appSecret, 30000, headers, querys, signHeader))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }
   
    }
}
