using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab7_WpfBinding.Models;

namespace Lab7_WpfBinding.Services
{
    public class CandidateService
    {
        private readonly List<Candidate> _candidates = new();


        public IEnumerable<Candidate> GetAll() => _candidates.AsReadOnly();


        public void Add(Candidate c) => _candidates.Add(c);


        public void Update(int index, Candidate c)
        {
            if (index >= 0 && index < _candidates.Count)
                _candidates[index] = c;
        }


        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _candidates.Count)
                _candidates.RemoveAt(index);
        }
    }
}
