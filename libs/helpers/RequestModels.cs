using System;
using System.Collections.Generic;

namespace PaymentsProviderApp.Models
{
    /// <summary>
    /// базовий запит
    /// </summary>
    public class BaseRequest
    {
        public string signature { get; set; }
    }


    /// <summary>
    /// запит на апі, 
    // !!!використовується для усії методів, які поступають на вхід!!!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Request<T> : BaseRequest
    {
        public T data { get; set; }
    }

    public class BaseResponse
    {
        public string statusMessage { get; set; } = "";
        public int statusCode { get; set; }
    }


    public class Response<T>: BaseResponse
    {
        public T? data { get; set; }

        public Response() { }

        public Response(int statusCode, string statusMessage)
        {
            this.statusCode = statusCode;
            this.statusMessage = statusMessage;

            data = default(T);
        }

        public Response(BaseResponse response)
        {
            this.statusCode = response.statusCode;
            this.statusMessage = response.statusMessage;

            data = default(T);
        }

        public Response(T data)
        {
            this.statusCode = 200;
            this.statusMessage = "success";

            this.data = data;
        }
    }

    public class ListRequest: PaginationRequest
    {
        public DateTime? dateFrom { get; set; } = null;
        public DateTime? dateTo { get; set; } = null;
        public string? sQuery { get; set; } = null;
        public bool onlyActive { get; set; } = true;
    }

    public class PaginationRequest
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 20;
    }
}