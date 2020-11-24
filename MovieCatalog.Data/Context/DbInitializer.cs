using MovieCatalog.Model.Models;
using System;
using System.Linq;

namespace MovieCatalog.Data.Context
{
    public static class DbInitializer
    {        
        public static void Initialize(MovieCatalogContext context)
        {
            context.Database.EnsureCreated();

            if (context.Films.Any())
            {
                return;  
            }

            var films = new Film[]
            {
                new Film {Title = "Зеленая миля",
                    Description = "В тюрьме для смертников появляется заключенный с божественным даром. Мистическая драма по роману Стивена Кинга",
                    RelaseDate = DateTime.Parse("1999-12-06"),
                    Director = "Фрэнк Дарабонт",
                    PosterPath = "green_mile.jpg"},

                new Film {Title = "Побег из Шоушенка",
                    Description = "Выдающаяся драма о силе таланта, важности дружбы, стремлении к свободе и Рите Хэйворт",
                    RelaseDate = DateTime.Parse("1994-09-10"),
                    Director = "Фрэнк Дарабонт",
                    PosterPath = "shawshank.jpg"},

                new Film {Title = "Властелин колец: Возвращение Короля",
                    Description = "Арагорн штурмует Мордор, а Фродо устал бороться с чарами кольца. Эффектный финал саги, собравший 11 «Оскаров»",
                    RelaseDate = DateTime.Parse("2003-12-01"),
                    Director = "Питер Джексон",
                    PosterPath = "lotr.jpg"},

                new Film {Title = "Интерстеллар",
                    Description = "Фантастический эпос про задыхающуюся Землю, космические полеты и парадоксы времени. «Оскар» за спецэффекты",
                    RelaseDate = DateTime.Parse("2014-10-26"),
                    Director = "Кристофер Нолан",
                    PosterPath = "interstellar.jpg"},

                new Film {Title = "Список Шиндлера",
                    Description = "Драма о холокосте и великом человеческом поступке",
                    RelaseDate = DateTime.Parse("1993-11-30"),
                    Director = "Стивен Спилберг",
                    PosterPath = "list.jpg"},

                new Film {Title = "Властелин колец: Братство кольца",
                    Description = "Экранизация бессмертных произведений Дж.Р.Р. Толкина",
                    RelaseDate = DateTime.Parse("2001-12-10"),
                    Director = "Питер Джексон",
                    PosterPath = "lotr2.jpg"},

                new Film {Title = "Форрест Гамп",
                    Description = "Том Хэнкс исполняет американскую мечту",
                    RelaseDate = DateTime.Parse("1994-06-23"),
                    Director = "Роберт Земекис",
                    PosterPath = "forrest.jpg"},

                new Film {Title = "Властелин колец: Две крепости",
                    Description = "Голлум ведет хоббитов в Мордор, а великие армии готовятся к битве. Два «Оскара»",
                    RelaseDate = DateTime.Parse("2002-12-05"),
                    Director = "Питер Джексон",
                    PosterPath = "lotr3.jpg"},

                new Film {Title = "1+1",
                    Description = "Бывший зек возвращает вкус к жизни чопорному аристократу, прикованному к инвалидному креслу",
                    RelaseDate = DateTime.Parse("2011-09-23"),
                    Director = "Оливье Накаш",
                    PosterPath = "oneplusone.jpg"},

                new Film {Title = "Король Лев",
                    Description = "Львенок Симба бросает вызов дяде-убийце. Величественный саундтрек, рисованная анимация и шекспировский размах",
                    RelaseDate = DateTime.Parse("1994-06-12"),
                    Director = "Роджер Аллерс",
                    PosterPath = "lionking.jpg"}
            };

            foreach (var film in films)
            {
                context.Films.AddRange(film);
            }

            context.SaveChanges();
        }
    }
}
