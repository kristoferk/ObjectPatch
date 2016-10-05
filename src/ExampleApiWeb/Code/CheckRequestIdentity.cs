using System;
using System.Net;

namespace ExampleApiWeb.Code
{

    public static class CheckRequestIdentity
    {
        public static void AssertEqualId(string queryId, string bodyId, Action<string> setBodyId)
        {
            if (string.IsNullOrEmpty(bodyId))
            {
                setBodyId(queryId);
            }
            else if (!string.Equals(queryId, bodyId, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Invalid request");
            }
        }

        public static void AssertEqualId(int queryId, int bodyId, Action<int> setBodyId)
        {
            if (bodyId < 1)
            {
                setBodyId(queryId);
            }
            else if (queryId != bodyId)
            {
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Invalid request");
            }
        }
    }
}
