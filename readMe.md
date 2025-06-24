# Advise the Tourist!

## Project Overview

"Advise the Tourist!" is a robust recommendation system designed to help users make informed decisions about places to visit, building upon the success of existing online rating platforms and leveraging social network interactions. Unlike traditional booking methods, this system empowers users to explore, rate, and comment on various locations, benefiting from the experiences and recommendations of others, including friends.

The core of the system is a well-designed database that meticulously stores user interactions, diverse rating types, and assessments of different places.

## Key Features

### Members

The system distinguishes between two types of members:

* **Normal Users:** Standard users of the system who can interact with places and other members.

* **Place Managers/Administrators:** Users who are responsible for managing specific place pages on the network.

All members can sign up using their email addresses. The system records essential user details, including first name, last name, phone number(s), nationality, and address.

**Friendship Functionality:**

* Members can add other members as friends.

* Friend requests must be accepted by the added person to confirm the friendship.

* Friends can view places each other have visited and send private messages.

**Place Liking:**

* Users can "like" a place they've visited without necessarily providing specific ratings.

### Places

"Advise the Tourist!" encompasses a wide range of place types to assist tourists globally:

* Cities

* Hotels

* Restaurants

* Museums

* Monuments

Each place includes:

* Name

* Building date (if known)

* Google Maps attributes (longitude and latitude) for recommendation purposes.

* Professional pictures uploaded by place managers/admins to attract visitors.

* User-uploaded images for liked or rated places.

**Specific Place Attributes:**

* **Cities:** Location, coastal city status (yes/no).

* **Restaurants:** Cuisine type, style.

* **Hotels:** Price of different room types (if known), available facilities and their descriptions (e.g., beach, pool, activities area).

* **Museums:** Opening and closing hours, ticket prices (if available).

* **Monuments:** A short description.

### Place Pages

Every place will have a dedicated page within the system. These pages will display:

* Information provided by the place's manager(s)/administrator(s).

* User-generated ratings and comments.

* A section for users to ask questions about the place.

### Rating System

The system supports diverse rating criteria tailored to different place types. Users can also add new rating criteria.

**Examples of Rating Criteria:**

* **Hotels:** Sleep Quality, Location, Rooms, Service, Value, Cleanliness (with ratings like Excellent, Very good, Average, Poor, Terrible).

* **Restaurants:** Food, Service, Value, Atmosphere.

### Comments

To facilitate quick decision-making, the system focuses on concise user feedback:

* Comments are limited to a maximum of 100 characters.

* Users can also use hashtag comments, with each hashtag limited to a maximum of 50 characters.

### Questions and Answers

* Any member can add questions on a place's page.

* Place managers/administrators are responsible for responding to these questions.

* All questions and their corresponding responses are viewable on the respective place pages.

## Technologies (To be determined/added during implementation)

* **Database:** MySQL

* **Full-Stack Framework:** ASP.NET