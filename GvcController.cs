using Backend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Data;

namespace gvcprj;

[ApiController]
[Route("api/gvc")]
[Authorize]
public class GvcController : ControllerBase
{
    private readonly ILogger<GvcController> _logger;
    private readonly GvcService _service;

    public GvcController(ILogger<GvcController> logger, GvcService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("getMatCategory")]
    public async Task<IActionResult> GetMatCategory([FromBody] IDictionary<string, object> data)
    {
        try
        {
            string requestURL = "https://gvc03.miraecit.com/api/itemPricePredict?";

            string timeStamp = RestUtil.ParseParam<string>(data, "timeStamp");
            string signature = RestUtil.ParseParam<string>(data, "signature");
            string startDate = RestUtil.ParseParam<string>(data, "startDate");
            string endDate = RestUtil.ParseParam<string>(data, "endDate");
            string item = RestUtil.ParseParam<string>(data, "item");
            string outFormat = RestUtil.ParseParam<string>(data, "outFormat");
            string period = RestUtil.ParseParam<string>(data, "period");

            string paramdata = $"nm_sday={startDate}&nm_eday={endDate}&nm_item={item}&nm_outformat={outFormat}&nm_frequency={period}"; //replace <value>

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("x-gvc-apigw-api-key", "a2d0bd1b-0992-4635-83cd-7c85db14e1ed");
            headers.Add("x-gvc-apigw-timestamp", timeStamp);
            headers.Add("x-gvc-apigw-signature", signature);

            var result = await HttpUtil.GetCall(requestURL + paramdata, headers);
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("interfaceData")]
    public async Task<IActionResult> InterfaceData([FromBody] IDictionary<string, object> datas)
    {
        try
        {
            string scenarioId = RestUtil.ParseParam<string>(datas, "scenarioId");
            string material = RestUtil.ParseParam<string>(datas, "material");
            string table = RestUtil.ParseParam<string>(datas, "table");
            var param = RestUtil.ParseParam<IDictionary<string, object>[]>(datas, "datas");

            var result = _service.InterfaceData(scenarioId, material, table, param).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("getDfOutput")]
    public async Task<IActionResult> GetDfOutput([FromBody] string scenarioId)
    {
        try
        {
            var result = await _service.GetDfOutput(scenarioId).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("getltOut")]
    public async Task<IActionResult> GetltOut([FromBody] IDictionary<string, object> data)
    {
        try
        {
            string scenarioId = RestUtil.ParseParam<string>(data, "scenarioId");
            DateTime startDate = RestUtil.ParseParam<DateTime>(data, "startDate");
            DateTime endDate = RestUtil.ParseParam<DateTime>(data, "endDate");
            string supplierId = RestUtil.ParseParam<string>(data, "supplierId");
            var result = await _service.GetltOut(scenarioId, startDate, endDate, supplierId).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("getDfResult")]
    public async Task<IActionResult> GetDfResult([FromBody] IDictionary<string, object> data)
    {
        try
        {
            string scenarioId = RestUtil.ParseParam<string>(data, "scenarioId");
            var result = await _service.GetDfResult(scenarioId).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("getEqpLoadStat")]
    public async Task<IActionResult> GetEqpLoadStat([FromBody] IDictionary<string, object> data)
    {
        try
        {
            string scenarioId = RestUtil.ParseParam<string>(data, "scenarioId");
            string startDate = RestUtil.ParseParam<string>(data, "startDate");
            string endDate = RestUtil.ParseParam<string>(data, "endDate");
            var result = await _service.GetEqpLoadStat(scenarioId, startDate, endDate).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
    
    [HttpPost("getPastLt")]
    public async Task<IActionResult> GetPastLt([FromBody] IDictionary<string, object> data)
    {
        try
        {
            string scenarioId = RestUtil.ParseParam<string>(data, "scenarioId");
            var result = await _service.GetPastLt(scenarioId).ConfigureAwait(false);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest();
        }
    }
}
