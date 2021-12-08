using System;

namespace Brenda.Core.Identifiers
{
    public static class Actions
    {
        public const string Redirect = "F0000001-A811-4C1B-B43A-A6ACAB4CFD6E";
        public const string Video = "F0000002-5879-4B95-8614-47E94411CA44";
        public const string Quiz = "F0000003-D469-4675-9D14-98C043255DAA";
        public const string Challenge = "F0000004-4715-4394-83DE-C6A6A64E59A8";

        public static bool IsRedirect(Asset asset) => asset != null && Redirect.Equals(asset.ActionId.ToString().ToUpper());
        public static bool IsRedirect(Campaign campaign) => Redirect.Equals(campaign.ActionId.ToString().ToUpper());
        public static bool IsRedirect(Guid id) => Redirect.Equals(id.ToString().ToUpper());

        public static bool IsVideo(Asset asset) => asset != null && Video.Equals(asset.ActionId.ToString().ToUpper());
        public static bool IsVideo(Campaign campaign) => Video.Equals(campaign.ActionId.ToString().ToUpper());
        public static bool IsVideo(Guid id) => Video.Equals(id.ToString().ToUpper());

        public static bool IsQuiz(Asset asset) => asset != null && Quiz.Equals(asset.ActionId.ToString().ToUpper());
        public static bool IsQuiz(Campaign campaign) => Quiz.Equals(campaign.ActionId.ToString().ToUpper());
        public static bool IsQuiz(Guid id) => Quiz.Equals(id.ToString().ToUpper());

        public static bool IsChallenge(Campaign campaign) => campaign != null && Challenge.Equals(campaign.ActionId.ToString().ToUpper());
        public static bool IsChallenge(Guid id) => Challenge.Equals(id.ToString().ToUpper());

        public static string NameOf(Guid id) =>
            id.ToString().ToUpper() switch
            {
                Redirect => "Redirect",
                Quiz => "Quiz",
                Video => "Video",
                Challenge => "Challenge",
                _ => string.Empty
            };
    }
}
