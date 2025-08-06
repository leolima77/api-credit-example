using ApiCredit.Models;
using ApiCredit.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
#if !DEBUG
using Microsoft.AspNetCore.Authorization;
#endif

namespace ApiCredit.Controllers
{
    [ApiController]
#if !DEBUG
    [Authorize]
#endif
    /// <summary>
    /// Controller that handles credit analysis requests.
    /// </summary>
    [Route("credit")]
    public class CreditController : ControllerBase
    {
        private readonly ICreditService _creditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditController"/> class.
        /// </summary>
        /// <param name="creditService">Service to handle credit analysis logic.</param>
        public CreditController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        /// <summary>
        /// Analyzes a credit request and returns the approval decision with risk index.
        /// Parameters
        /// UnemploymentTax: 7.5 (%)
        /// Inflation: 4.2 (%)
        /// CreditHistory: 8 (scale: 0 to 10)
        /// Debts: 1000 (euros)
        /// </summary>
        /// <param name="creditAnalysisRequest">The request containing financial indicators and customer information.</param>
        /// <returns>Returns risk index and whether the credit is approved.</returns>
        [HttpPost]
        [ProducesResponseType<CreditAnalysisResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<object>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<object>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<object>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreditAnalysis([FromBody] CreditAnalysisRequest creditAnalysisRequest)
        {

            var result = _creditService.AskForCredit(
                creditAnalysisRequest.TaxId,
                creditAnalysisRequest.UnemploymentTax,
                creditAnalysisRequest.Inflation,
                creditAnalysisRequest.CreditHistory,
                creditAnalysisRequest.Debts
            );

            return Ok(result);
        }
    }

}
