using aliyun_api_gateway_sdk.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aliyun_api_gateway_sdk.Util
{
    class SignUtil
    {
        public static string Sign(String method, String url, String sectet, Dictionary<String, String> headers, Dictionary<String, String> bodyDict, List<String> signHeaderPrefixList)
        {
            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA256"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(sectet.ToCharArray());
                String signStr = BuildStringToSign(headers, url, bodyDict, method, signHeaderPrefixList);
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(signStr.ToCharArray())));
            }
        }

        private static String BuildStringToSign(Dictionary<String, String> headers, String url, Dictionary<String, String> bodyDict, String method, List<String> signHeaderPrefixList)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(method.ToUpper()).Append(Constants.LF);
            if (headers.ContainsKey(HttpHeader.HTTP_HEADER_ACCEPT) && headers[HttpHeader.HTTP_HEADER_ACCEPT] != null)
            {
                sb.Append(headers[HttpHeader.HTTP_HEADER_ACCEPT]);
            }
            sb.Append(Constants.LF);
            if (headers.ContainsKey(HttpHeader.HTTP_HEADER_CONTENT_MD5) && headers[HttpHeader.HTTP_HEADER_CONTENT_MD5] != null)
            {
                sb.Append(headers[HttpHeader.HTTP_HEADER_CONTENT_MD5]);
            }
            sb.Append(Constants.LF);
            if (headers.ContainsKey(HttpHeader.HTTP_HEADER_CONTENT_TYPE) && headers[HttpHeader.HTTP_HEADER_CONTENT_TYPE] != null)
            {
                sb.Append(headers[HttpHeader.HTTP_HEADER_CONTENT_TYPE]);
            }
            sb.Append(Constants.LF);
            if (headers.ContainsKey(HttpHeader.HTTP_HEADER_DATE) && headers[HttpHeader.HTTP_HEADER_DATE] != null)
            {
                sb.Append(headers[HttpHeader.HTTP_HEADER_DATE]);
            }
            sb.Append(Constants.LF);
            sb.Append(BuildHeaders(headers, signHeaderPrefixList));
            sb.Append(BuildResource(url, bodyDict));

            return sb.ToString();
        }

        /**
         * 构建待签名Path+Query+FormParams
         *
         * @param url          Path+Query
         * @param formParamMap POST表单参数
         * @return 待签名Path+Query+FormParams
         */
        private static String BuildResource(String url, Dictionary<String, String> bodyDict)
        {
            if (url.Contains("?")) {
                String path = url.Split('?')[0];
                String queryString = url.Split('?')[1];
                url = path;
                if (bodyDict == null)
                {
                    bodyDict = new Dictionary<String, String>();
                }
                if (!String.IsNullOrEmpty(queryString)) {
                    foreach (var query in queryString.Split('&'))
                    {
                        String key = query.Split('=')[0];
                        String value = "";
                        if (query.Split('=').Length == 2) {
                            value = query.Split('=')[1];
                        }
                        if (bodyDict.ContainsKey(key) == false)
                        {
                            bodyDict.Add(key, value);
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(url);

            if (bodyDict != null && bodyDict.Count > 0)
            {
                sb.Append('?');

                //参数Key按字典排序
                IDictionary<String, String> sortDict = new SortedDictionary<String, String>(bodyDict, StringComparer.Ordinal);
          
                int flag = 0;
                foreach (var str in sortDict) {
                    if (flag != 0) {
                        sb.Append('&');
                    }

                    flag++;
                    String key = str.Key;
                    String val = str.Value;

                    if (String.IsNullOrEmpty(val)) {
                        sb.Append(key);
                    } else {
                        sb.Append(key).Append("=").Append(val);
                    }
                }
            }

            return sb.ToString();
        }


        /**
        * 构建待签名Http头
        *
        * @param headers              请求中所有的Http头
        * @param signHeaderPrefixList 自定义参与签名Header前缀
        * @return 待签名Http头
        */
        private static String BuildHeaders(Dictionary<String, String> headers, List<String> signHeaderPrefixList) {
            Dictionary<String, String> headersToSign = new Dictionary<String, String>();

            if (headers != null) {
                StringBuilder signHeadersStringBuilder = new StringBuilder();

                int flag = 0;
                foreach  (var header in headers) {
                    if (IsHeaderToSign(header.Key, signHeaderPrefixList)) {
                        if (flag != 0) {
                            signHeadersStringBuilder.Append(",");
                        }
                        flag++;
                        signHeadersStringBuilder.Append(header.Key);
                        headersToSign.Add(header.Key, header.Value);
                    }
                }

                headers.Add(SystemHeader.X_CA_SIGNATURE_HEADERS, signHeadersStringBuilder.ToString());
            }

            IDictionary<String, String> sortedDict = new SortedDictionary<String, String>(headersToSign, StringComparer.Ordinal);

            StringBuilder sb = new StringBuilder();
            foreach (var val in sortedDict)
            {
                sb.Append(val.Key).Append(':').Append(val.Value).Append(Constants.LF);
            }

            return sb.ToString();
        }


        /**
        * Http头是否参与签名
        * return
        */
        private static bool IsHeaderToSign(String headerName, List<String> signHeaderPrefixList) {
            if (String.IsNullOrEmpty(headerName) ) {
                return false;
            }

            if (headerName.StartsWith(Constants.CA_HEADER_TO_SIGN_PREFIX_SYSTEM)) {
                return true;
            }

            if (signHeaderPrefixList != null) {
                foreach (var signHeaderPrefix in signHeaderPrefixList)
                {
                    if (headerName.StartsWith(signHeaderPrefix))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
