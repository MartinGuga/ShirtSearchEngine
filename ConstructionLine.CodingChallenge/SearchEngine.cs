using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly List<SizeCount>_sizeCounts;
        private readonly List<ColorCount>_colorCounts;
        private readonly List<Shirt> _listOfShirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _listOfShirts = new List<Shirt>();
            _sizeCounts = new List<SizeCount>();
            _colorCounts = new List<ColorCount>();

        }


        public SearchResults Search(SearchOptions options)
        {
            foreach (var shirt in _shirts)
            {
                foreach (var color in options.Colors)
                {
                    if (color.Name == shirt.Name)
                    {
                        _listOfShirts.Add(shirt);
                    }

                }
            }

            Parallel.ForEach(Size.All, size =>
            {
                _sizeCounts.Add(new SizeCount
                {
                    Count = _shirts.Count(s => s.Size.Id == size.Id
                                               && (!options.Colors.Any() || options.Colors.Select(c => c.Id).Contains(s.Color.Id))),
                    Size = size
                });
            });

            Parallel.ForEach(Color.All, color =>
            {
                _colorCounts.Add(new ColorCount
                {
                    Count = _shirts.Count(s => s.Color == color
                                               && (!options.Sizes.Any() ||
                                                   options.Sizes.Select(c => c.Id).Contains(s.Size.Id))),
                    Color = color
                });
            });

            

            return new SearchResults
            {
                Shirts = _listOfShirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }
    }
}