/*
namespace DemoBackendArchitecture.API.Controllers;

public class TempCOntorller
{
    const generateChecksum = (input, privateKey) => {
        const currentTime = Math.floor(Date.now() / 1000); // Get current timestamp in seconds
        const dataWithTime = input + currentTime;
        const checksum = crypto.createHmac('sha256', privateKey).update(dataWithTime).digest('hex'); // Use HMAC with the secret key
        return `${checksum}:${currentTime}`;
    }

    const verifyChecksum = (checksum, input, privateKey, validityPeriodSeconds) => {
        const [input, timestamp] = checksum.split(':');
        const currentTime = Math.floor(Date.now() / 1000);
    
        if (currentTime - parseInt(timestamp) > validityPeriodSeconds) {
            return false; // Checksum has expired
        }

        const dataWithTime = input + timestamp;
        const expectedHash = crypto.createHmac('sha256', privateKey).update(dataWithTime).digest('hex'); // Use HMAC with the secret key
        return hash === expectedHash;
    }
if (!verifyChecksum(req.body.checksum, `PublicKey1${req.body.campusCode}PublicKey2`, "40612359b37772321b7", 60)) {
    return res.status(422).json({
        message: "CHECK_SUM IS NOT VALID.",
    });
}

//Middleware
namespace APIs.ActionFilter
{
    public class CheckIpFilterAttribute: ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private readonly string _safeList;
    
        public CheckIpFilterAttribute(string safeList, ILogger logger)
        {
            _logger = logger;
            _safeList = safeList;
        }
    
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;

            if (remoteIp == null)
            {
                _logger.LogWarning("REMOTE IP NOT FOUND");

                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                
                return;
            }
            
            _logger.LogDebug("REMOTE IP ADDRESS: {RemoteIp}", remoteIp);
            
            var ip = _safeList.Split(';');
            
            var badIp = true;
            
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }
            
            foreach (var address in ip)
            {
                var testIp = IPAddress.Parse(address);
                
                if (testIp.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }
    
            if (badIp)
            {
                _logger.LogWarning("FORBIDDEN REQUEST FROM IP: {RemoteIp}", remoteIp);
                
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                
                return;
            }
    
            base.OnActionExecuting(context);
        }
    }
}
//Model validation
private readonly string _value;

public AddHeaderAttribute(string name, string value)
{
    _name = name;
    _value = value;
}

public override void OnResultExecuting(ResultExecutingContext context)
{
    context.HttpContext.Response.Headers.Add(_name, new[] { _value });

    base.OnResultExecuting(context);
}
}

//Exception
{status: int, message: string, data: T,  timestamp: unix of epoch second, }
}
*/