﻿using System.Collections.Generic;
using HomesEngland.Boundary.UseCase;
using HomesEngland.UseCase.CreateAsset.Models;

namespace HomesEngland.UseCase.CreateAsset
{
    public interface IBulkCreateAssetUseCase : IAsyncUseCaseTask<IList<CreateAssetRequest>, IList<CreateAssetResponse>>
    {
    }
}
