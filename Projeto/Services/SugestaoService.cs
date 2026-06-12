using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projeto.Models;

namespace Projeto.Services
{
    public class SugestaoService
    {
        private readonly List<Sugestao> _sugestoes = new();

        public IEnumerable<Sugestao> GetAll() => _sugestoes;

        public void Add(Sugestao sugestao)
        {
            sugestao.Id = Guid.NewGuid();
            _sugestoes.Add(sugestao);
        }

        public void Remove(Guid id)
        {
            var sugestao = _sugestoes.FirstOrDefault(s => s.Id == id);
            if (sugestao != null)
            {
                _sugestoes.Remove(sugestao);
            }
        }
    }
}