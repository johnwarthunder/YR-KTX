using System.Text;

namespace Tour {

    class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            List<Versenyzo> versenyzok = new List<Versenyzo>();
            foreach (string line in File.ReadLines("tour.csv")) {                
                if (line.StartsWith("szakasz")) continue;
                string[] parts = line.Split(";");
                versenyzok.Add(new Versenyzo(

                    int.Parse(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4]
                ));
            }

            int fifthRoundCount = versenyzok.Count(index => index.szakasz == 5);
            Console.WriteLine($"4. Feladat: 5-ös szakasz versenyzői: {fifthRoundCount} fő");

            var fifthRoundComps = versenyzok.Where(index => index.szakasz == 5).ToList();
            double averageTimeForAll = fifthRoundComps.Average(index => index.returnTime(index.ido));
            int overAverageComps = fifthRoundComps.Count(index => index.returnTime(index.ido) > averageTimeForAll);
            Console.WriteLine($"5. Feladat: Átlag felett: {overAverageComps} fő");

            var averageForTeams = versenyzok.GroupBy(index => index.csapat)
                                            .Select(selectIndex => new { nev = selectIndex.Key, atlag = selectIndex.Average(timeIndex => timeIndex.returnTime(timeIndex.ido)) })
                                            .OrderBy(x => x.nev);

            Console.WriteLine("6. Feladat: Csapatonkénti átlagos idő:");
            foreach (var average in averageForTeams) {
                Console.WriteLine($"{average.nev} : {average.atlag}");
            }

            Console.WriteLine($"7. feladat: Kitüntetett versenyzők: {versenyzok.Count(index => index.returnAward(index.nemzetiseg, index.ido))} ");

            var top10SelectedComp = fifthRoundComps.OrderBy(index => index.returnTime(index.ido)).Take(10);
            foreach (var index in top10SelectedComp) {
                Console.WriteLine($"{index.nev}, {index.ido}");
            }
        }
    }

    class Versenyzo {
        public int szakasz { get; set; }
        public string ido { get; set; }
        public string nev { get; set; }
        public string nemzetiseg { get; set; }
        public string csapat { get; set; }

        public Versenyzo(int szakasz, string ido, string nev, string nemzetiseg, string csapat)
        {
            this.szakasz = szakasz;
            this.ido = ido;
            this.nev = nev;
            this.nemzetiseg = nemzetiseg;
            this.csapat = csapat;
        }

        public int returnTime(string time) {
            string[] parts = time.Split(":");
            return int.Parse(parts[0]) * 3600 + int.Parse(parts[1]) * 60 + int.Parse(parts[2]);
        }
        public bool returnAward(string competitor, string time)
        {
            return (competitor == "USA" && returnTime(time) < 10800);
        }
    }

}
