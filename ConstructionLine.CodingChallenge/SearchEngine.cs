using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<ColorCount> _colorCounts;
        private readonly List<Shirt> _listOfShirts;
        private readonly List<Shirt> _shirts;
        private readonly List<SizeCount> _sizeCounts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _listOfShirts = new List<Shirt>();
            _sizeCounts = new List<SizeCount>();
            _colorCounts = new List<ColorCount>();
        }

        public SearchResults Search(SearchOptions options)
        {
            CalculateShirts(options);

            CalculateSizeCount(options);

            CalcaluteColorCount(options);

            return new SearchResults
            {
                Shirts = _listOfShirts,
                SizeCounts = _sizeCounts,
                ColorCounts = _colorCounts
            };
        }

        private void CalcaluteColorCount(SearchOptions options)
        {
            //This method calculates the color found by the options provided.
            Parallel.ForEach(Color.All, color =>
            {
                _colorCounts.Add(new ColorCount
                {
                    Count = _shirts.Count(c => c.Color.Id == color.Id
                                               && (!options.Sizes.Any() ||
                                                   options.Sizes.Select(s => s.Id).Contains(c.Size.Id))),
                    Color = color
                });
            });
        }

        private void CalculateSizeCount(SearchOptions options)
        {
            //This method calculates the sizes found by the options provided.
            Parallel.ForEach(Size.All, size =>
            {
                _sizeCounts.Add(new SizeCount
                {
                    Count = _shirts.Count(s => s.Size.Id == size.Id
                                               && (!options.Colors.Any() ||
                                                   options.Colors.Select(c => c.Id).Contains(s.Color.Id))),
                    Size = size
                });
            });
        }

        private void CalculateShirts(SearchOptions options)
        {
            //This method calculates the shirts found by the color options provided.
            foreach (var shirt in _shirts)
            {
                foreach (var color in options.Colors)
                    if (color.Name == shirt.Name)
                        _listOfShirts.Add(shirt);
            }
        }
    }
}