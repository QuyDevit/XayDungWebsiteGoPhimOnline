using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BestTyping
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "competitions",
                url: "competitiontest/{codejoin}",
                defaults: new { controller = "Competition", action = "Competitions", codejoin = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "CheckTyping",
               url: "typing-test",
               defaults: new { controller = "Home", action = "CheckTyping" }
           );
            routes.MapRoute(
              name: "CheckTypingCustom",
              url: "typing-custom",
              defaults: new { controller = "Home", action = "CheckTypingCustom" }
          );
            routes.MapRoute(
               name: "CheckTypingAdvanced",
               url: "typing-test-hard",
               defaults: new { controller = "Home", action = "CheckTypingAdvanced" }
           );
            routes.MapRoute(
                name: "CompetitionsTyping",
                url: "competitions",
                defaults: new { controller = "Home", action = "CompetitionsTyping" }
            );

            routes.MapRoute(
               name: "MyTextPractice",
               url: "text-practice/my-texts",
               defaults: new { controller = "Home", action = "MyTextPractice" }
            );
            routes.MapRoute(
               name: "TextPractice",
               url: "text-practice",
               defaults: new { controller = "Home", action = "TextPractice" }
            );
            routes.MapRoute(
               name: "TextPracticeNew",
               url: "text-practice/upcomping",
               defaults: new { controller = "Home", action = "TextPracticeNew" }
            );
            routes.MapRoute(
               name: "TextPracticeLike",
               url: "text-practice/my-favorite",
               defaults: new { controller = "Home", action = "TextPracticeLike" }
            );
            routes.MapRoute(
               name: "TextPracticeTest",
               url: "text-practice-test/{codejoin}-{rank}/{name}",
               defaults: new { controller = "Home", action = "TextPracticeTest", codejoin = UrlParameter.Optional, rank = UrlParameter.Optional, name = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Settings",
              url: "setting",
              defaults: new { controller = "Home", action = "Settings" }
            );

            routes.MapRoute(
              name: "CreateTextPractice",
              url: "text-practice/create",
              defaults: new { controller = "Home", action = "CreateTextPractice" }
            );
            routes.MapRoute(
             name: "EditTextPractice",
             url: "text-practice/edit/{codejoin}",
             defaults: new { controller = "Home", action = "EditTextPractice", codejoin = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
