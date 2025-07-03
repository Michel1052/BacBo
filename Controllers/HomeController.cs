using System.Diagnostics;
using BacBo.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace BacBo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;                

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult BacBo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Play(string choice)
        {
            var rand = new Random();

            var playerDice1 = rand.Next(1, 7);
            var playerDice2 = rand.Next(1, 7);
            var bankerDice1 = rand.Next(1, 7);
            var bankerDice2 = rand.Next(1, 7);

            var playerTotal = playerDice1 + playerDice2;
            var bankerTotal = bankerDice1 + bankerDice2;

            string result;
            if (playerTotal > bankerTotal) result = "Player";
            else if (playerTotal < bankerTotal) result = "Banker";
            else result = "Tie";           
            


            var bet = new Bet
            {
                Choice = choice,
                PlayerDice1 = playerDice1,
                PlayerDice2 = playerDice2,
                BankerDice1 = bankerDice1,
                BankerDice2 = bankerDice2,
                Result = result                
            };

            var adm = new PainelADM
            {
                Aposta = choice,
                Dado1dojogador = playerDice1,
                Dado2dojogador = playerDice2,
                Dado1dabanca = bankerDice1,
                Dado2dabanca = bankerDice2,
                Resultado = result
            };

            if(choice == result)
            {
                adm.Status = "Vitória";
            }

            else
            {
                adm.Status = "Derrota";
            }


                //string jsonBets = HttpContext.Session.GetString("betHistory");

                List<Bet> betList = new List<Bet>();

            /*if (!string.IsNullOrEmpty(jsonBets))
            {
                betList = JsonSerializer.Deserialize<List<Bet>>(jsonBets);
            }
            else
            {
                betList = new List<Bet>();
            }*/

            
            betList.Add(bet);

            
            //jsonBets = JsonSerializer.Serialize(betList);
            //HttpContext.Session.SetString("betHistory", jsonBets);

            PainelADM(adm);

            return View("BacBo", betList);
        }
        
        public IActionResult PainelADM(PainelADM adm)
        {
            string jsonBets = HttpContext.Session.GetString("betHistory");

            List<PainelADM> betList;
                        

                if (!string.IsNullOrEmpty(jsonBets))
                {
                    betList = JsonSerializer.Deserialize<List<PainelADM>>(jsonBets);
                }
                else
                {
                    betList = new List<PainelADM>();
                }

            if (adm.Aposta != null)
            {
                

                betList.Add(adm);


                jsonBets = JsonSerializer.Serialize(betList);
                HttpContext.Session.SetString("betHistory", jsonBets);
                
                
            }

            ViewBag.TotaldeApostas = betList.Count;

            int vit = 0;

            int derr = 0;

            int player = 0;

            int banker = 0;

            int tie = 0;

            decimal winrate = 0;

            decimal lossrate = 0;

            decimal playerrate = 0;

            decimal bankerrate = 0;

            decimal tierate = 0;

            foreach (var bet in betList)
            {
                if(bet.Status == "Vitória")
                {
                    vit++;
                }

                else
                {
                    derr++;
                }

                if (bet.Resultado == "Player")
                {
                    player++;
                }

                if (bet.Resultado == "Banker")
                {
                    banker++;
                }

                if (bet.Resultado == "Tie")
                {
                    tie++;
                }
            }

            winrate = (decimal)vit * 100m / (decimal)ViewBag.TotaldeApostas;

            lossrate = (decimal)derr * 100m / (decimal)@ViewBag.TotaldeApostas;

            playerrate = (decimal)player * 100m / (decimal)@ViewBag.TotaldeApostas;

            bankerrate = (decimal)banker * 100m / (decimal)@ViewBag.TotaldeApostas;

            tierate = (decimal)tie * 100m / (decimal)@ViewBag.TotaldeApostas;

            ViewBag.TotaldeVitorias = vit;

            ViewBag.TotaldeDerrotas = derr;

            ViewBag.TotalPlayer = player;

            ViewBag.TotalBanker = banker;

            ViewBag.TotalTie = tie;

            ViewBag.Winrate = Math.Round(winrate, 1, MidpointRounding.AwayFromZero);

            ViewBag.Lossrate = Math.Round(lossrate, 1, MidpointRounding.AwayFromZero);

            ViewBag.Playerrate = Math.Round(playerrate, 1, MidpointRounding.AwayFromZero);

            ViewBag.Bankerrate = Math.Round(bankerrate, 1, MidpointRounding.AwayFromZero);

            ViewBag.Tierate = Math.Round(tierate, 1, MidpointRounding.AwayFromZero);

            return View("PainelADM", betList);
        }        

        public IActionResult Resultados(string resultado)
        {
            string voce = "Você";
            string banca = "Banca";

            var adm = new PainelADM();

            if (resultado == "Player")
            {
                
                adm.Status = "Vitória";
            }

            if (resultado == "Player")
            {
                adm.Status = "Vitória";
            }            

            return View();
        }
    }
}

