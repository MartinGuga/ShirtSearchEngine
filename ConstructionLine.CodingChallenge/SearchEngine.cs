using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly  List<SizeCount>_sizeCounts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }


        public SearchResults Search(SearchOptions options)
        {
            Parallel.ForEach(Size.All, size =>
            {
                _sizeCounts.Add(new SizeCount
                {
                    Count = _shirts.Count(s => s.Size == size
                                               && (!options.Colors.Any() ||
                                                   options.Colors.Select(c => c.Id).Contains(s.Color.Id))),
                    Size = size
                });
            });

            return new SearchResults
            {
            };
        }
    }
}