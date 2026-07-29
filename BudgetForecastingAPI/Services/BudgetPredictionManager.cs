using System;
using System.Linq;
using BudgetForecastingAPI.DTOs;

namespace BudgetForecastingAPI.Services
{
    public class BudgetPredictionManager : IBudgetPredictionService
    {
        private readonly decimal _enflasyonAgirligi;
        private readonly decimal _dolarAgirligi;
        private readonly decimal _altinAgirligi;

        public BudgetPredictionManager(
            decimal enflasyonAgirligi = 0.5m,
            decimal dolarAgirligi = 0.3m,
            decimal altinAgirligi = 0.2m)
        {
            _enflasyonAgirligi = enflasyonAgirligi;
            _dolarAgirligi = dolarAgirligi;
            _altinAgirligi = altinAgirligi;
        }

        public BudgetPredictionResponseDTO PredictBudget(BudgetPredictionRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.GecmisButceler == null || !request.GecmisButceler.Any())
                throw new ArgumentException("Gecmis butce verisi bos olamaz.", nameof(request));

            if (request.GecmisEkonomikGosterge == null || request.BeklenenEkonomikGosterge == null)
                throw new ArgumentException("Ekonomik gosterge verileri eksik.", nameof(request));

            var sonKayit = request.GecmisButceler.OrderBy(b => b.Year).Last();
            decimal oncekiYilHarcamasi = sonKayit.ActualSpent;
            int tahminYili = sonKayit.Year + 1;

            decimal dolarDegisimOrani = HesaplaYuzdeDegisim(
                request.GecmisEkonomikGosterge.UsdExchangeRate,
                request.BeklenenEkonomikGosterge.UsdExchangeRate);

            decimal altinDegisimOrani = HesaplaYuzdeDegisim(
                request.GecmisEkonomikGosterge.GoldPriceGram,
                request.BeklenenEkonomikGosterge.GoldPriceGram);

            decimal enflasyonOrani = request.BeklenenEkonomikGosterge.InflationRate / 100m;

            decimal bilesikDegisimOrani =
                (enflasyonOrani * _enflasyonAgirligi) +
                (dolarDegisimOrani * _dolarAgirligi) +
                (altinDegisimOrani * _altinAgirligi);

            decimal tahminiButce = oncekiYilHarcamasi * (1 + bilesikDegisimOrani);

            return new BudgetPredictionResponseDTO
            {
                DepartmentName = request.DepartmentName,
                TahminYili = tahminYili,
                OncekiYilHarcamasi = oncekiYilHarcamasi,
                TahminiButce = Math.Round(tahminiButce, 2),
                YuzdeDegisim = Math.Round(bilesikDegisimOrani * 100, 2),
                Aciklama = $"Enflasyon: %{enflasyonOrani * 100:0.##}, Dolar degisimi: %{dolarDegisimOrani * 100:0.##}, Altin degisimi: %{altinDegisimOrani * 100:0.##} agirlikli katsayilarla hesaplandi."
            };
        }

        private static decimal HesaplaYuzdeDegisim(decimal eskiDeger, decimal yeniDeger)
        {
            if (eskiDeger == 0)
                return 0;

            return (yeniDeger - eskiDeger) / eskiDeger;
        }
    }
}
