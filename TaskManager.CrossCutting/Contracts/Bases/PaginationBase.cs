using Swashbuckle.AspNetCore.Annotations;

namespace TaskManager.CrossCutting.Contracts.Bases
{
    public class PaginationBase
    {
        [SwaggerSchema(Description = "Número total de itens na lista.")]
        public int TotalCount { get; set; }
        [SwaggerSchema(Description = "Número de itens por página.")]
        public int PageSize { get; set; } = 10;
        [SwaggerSchema(Description = "Número da página atual.")]
        public int CurrentPage { get; set; } = 1;
    }
}
