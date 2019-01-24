using System.Collections.Generic;

namespace HomesEngland.UseCase.Models
{
    public interface IResponse<T>
    {
        IList<T> ToCsv();
    }
}
