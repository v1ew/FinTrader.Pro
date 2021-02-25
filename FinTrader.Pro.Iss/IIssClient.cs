using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinTrader.Pro.Iss
{
    public interface IIssClient
    {
        Task<TResponseDto> Get<TResponseDto>(string engine, string market, string args);
    }
}
