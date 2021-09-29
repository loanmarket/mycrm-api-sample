// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The AdviserAttributes. </summary>
    public partial class AdviserAttributes
    {
        /// <summary> Initializes a new instance of AdviserAttributes. </summary>
        public AdviserAttributes()
        {
        }

        /// <summary> Initializes a new instance of AdviserAttributes. </summary>
        /// <param name="googlePlaces"></param>
        /// <param name="created"></param>
        /// <param name="myLeadGenActivationDate"></param>
        /// <param name="bio"></param>
        /// <param name="jobTitle"></param>
        /// <param name="isMyLeadGenActive"></param>
        /// <param name="status"></param>
        /// <param name="email"></param>
        /// <param name="skype"></param>
        /// <param name="facebook"></param>
        /// <param name="linkedIn"></param>
        /// <param name="twitter"></param>
        /// <param name="youtubeFeatured"></param>
        /// <param name="calendly"></param>
        /// <param name="myLeadGenerator"></param>
        /// <param name="profilePhotoHeadShot"></param>
        /// <param name="profilePhotoHalfBody"></param>
        /// <param name="profilePhotoFullBody"></param>
        /// <param name="youtubeChannel"></param>
        internal AdviserAttributes(string googlePlaces, DateTimeOffset? created, DateTimeOffset? myLeadGenActivationDate, string bio, string jobTitle, bool? isMyLeadGenActive, string status, string email, string skype, string facebook, string linkedIn, string twitter, string youtubeFeatured, string calendly, string myLeadGenerator, string profilePhotoHeadShot, string profilePhotoHalfBody, string profilePhotoFullBody, string youtubeChannel)
        {
            GooglePlaces = googlePlaces;
            Created = created;
            MyLeadGenActivationDate = myLeadGenActivationDate;
            Bio = bio;
            JobTitle = jobTitle;
            IsMyLeadGenActive = isMyLeadGenActive;
            Status = status;
            Email = email;
            Skype = skype;
            Facebook = facebook;
            LinkedIn = linkedIn;
            Twitter = twitter;
            YoutubeFeatured = youtubeFeatured;
            Calendly = calendly;
            MyLeadGenerator = myLeadGenerator;
            ProfilePhotoHeadShot = profilePhotoHeadShot;
            ProfilePhotoHalfBody = profilePhotoHalfBody;
            ProfilePhotoFullBody = profilePhotoFullBody;
            YoutubeChannel = youtubeChannel;
        }

        /// <summary> Gets the google places. </summary>
        public string GooglePlaces { get; }
        /// <summary> Gets the created. </summary>
        public DateTimeOffset? Created { get; }
        /// <summary> Gets the my lead gen activation date. </summary>
        public DateTimeOffset? MyLeadGenActivationDate { get; }
        /// <summary> Gets the bio. </summary>
        public string Bio { get; }
        /// <summary> Gets the job title. </summary>
        public string JobTitle { get; }
        /// <summary> Gets the is my lead gen active. </summary>
        public bool? IsMyLeadGenActive { get; }
        /// <summary> Gets the status. </summary>
        public string Status { get; }
        /// <summary> Gets the email. </summary>
        public string Email { get; }
        /// <summary> Gets the skype. </summary>
        public string Skype { get; }
        /// <summary> Gets the facebook. </summary>
        public string Facebook { get; }
        /// <summary> Gets the linked in. </summary>
        public string LinkedIn { get; }
        /// <summary> Gets the twitter. </summary>
        public string Twitter { get; }
        /// <summary> Gets the youtube featured. </summary>
        public string YoutubeFeatured { get; }
        /// <summary> Gets the calendly. </summary>
        public string Calendly { get; }
        /// <summary> Gets the my lead generator. </summary>
        public string MyLeadGenerator { get; }
        /// <summary> Gets the profile photo head shot. </summary>
        public string ProfilePhotoHeadShot { get; }
        /// <summary> Gets the profile photo half body. </summary>
        public string ProfilePhotoHalfBody { get; }
        /// <summary> Gets the profile photo full body. </summary>
        public string ProfilePhotoFullBody { get; }
        /// <summary> Gets the youtube channel. </summary>
        public string YoutubeChannel { get; }
    }
}
