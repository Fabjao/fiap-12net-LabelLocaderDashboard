using GeekBurger.LabelLocader.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabelLoader.Negocio
{
    public interface IProdutoNegocio
    {
        Task<Operacao> ListaDeProduto();
    }
}
