using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.IAP
{
  public interface IIAPService : IService
  {
    bool IsInitialized { get; }
    List<ProductDescription> Products { get; }
    event Action Initialized;
    void Initialize();
    void StartPurchase(string productId);
  }
}