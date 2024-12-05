using Newtonsoft.Json;
using MovieStreamingService.Domain.Models;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MovieStreamingService.Application.Services;

public class PaymentService
{
    private readonly string _publicKey;
    private readonly string _privateKey;

    public PaymentService(string liqPublicKey, string liqPrivateKey)
    {
        _publicKey = liqPublicKey;
        _privateKey = liqPrivateKey;
    }

    private static string GenerateSignature(string data)
    {
        var sha1 = new System.Security.Cryptography.SHA1Managed();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }

    public async Task<bool> ProcessPayment(Subscription subscription, int months, string googlePayToken)
    {
        var liqPayData = new
        {
            public_key = _publicKey,
            paytype = "gpay",
            gpay_token = googlePayToken,
            version = 3,
            action = "pay",
            amount = subscription.Price*months,
            currency = "UAH",
            description = $"Оплата підписки {subscription.Name} на {months} місяць(ів)",
            order_id = Guid.NewGuid().ToString()
        };

        var jsonData = JsonConvert.SerializeObject(liqPayData);
        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
        var signature = GenerateSignature(_privateKey + data + _privateKey);

        var client = new HttpClient();
        var postData = new Dictionary<string, string>
        {
            { "data", data },
            { "signature", signature }
        };

        var response = await client.PostAsync("https://www.liqpay.ua/api/request", new FormUrlEncodedContent(postData));
        var content = response.Content.ReadAsStringAsync().Result;
        var contentJson = JsonConvert.DeserializeObject<dynamic>(content);
        var status = contentJson["status"].ToString();

        return response.IsSuccessStatusCode && status == "success";
    }
}