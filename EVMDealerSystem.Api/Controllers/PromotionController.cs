﻿using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Promotion;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : BaseApiController
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost]
        public async Task<ActionResult<Result<PromotionResponse>>> CreatePromotion([FromBody] PromotionCreateRequest request)
        {
            var result = await _promotionService.CreatePromotionAsync(request);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<PromotionResponse>>>> GetAllPromotions()
        {
            var result = await _promotionService.GetAllPromotionsAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<PromotionResponse>>> GetPromotionById(Guid id)
        {
            var result = await _promotionService.GetPromotionByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<PromotionResponse>>> UpdatePromotion(Guid id, [FromBody] PromotionUpdateRequest request)
        {
            var result = await _promotionService.UpdatePromotionAsync(id, request);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeletePromotion(Guid id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            return HandleResult(result);
        }
    }
}
