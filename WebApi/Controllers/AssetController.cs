﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HomesEngland.Boundary.UseCase;
using HomesEngland.Exception;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IGetAssetUseCase _assetUseCase;
        public AssetController(IGetAssetUseCase useCase)
        {
            _assetUseCase = useCase;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        { 
            try
            {
                Dictionary<string,string> result = await _assetUseCase.Execute(id);
                return ConvertToJson(result);   
            }
            catch (NoAssetException)
            {
                return "{}";
            }
          
        }

        private static string ConvertToJson(Dictionary<string, string> result)
        {
            string quoteMark = "\"";
            string expectedResult = "{";
            foreach (string key in result.Keys)
            {
                expectedResult += quoteMark + key + quoteMark + ":" + quoteMark + result[key] + quoteMark+",";
            }
            expectedResult =  RemoveLastComma(expectedResult)+"}";
            return expectedResult;
        }
        
        private static string RemoveLastComma(string expectedResult)
        {
            return expectedResult.Remove(expectedResult.Length - 1);
        }
    }
}
