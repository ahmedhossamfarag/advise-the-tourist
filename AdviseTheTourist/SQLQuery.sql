--create proc GetFriend 
--@email varchar(50),
--@friends_fn varchar(50) out,
--@friends_ln varchar(50) out,
--@friends_em varchar(50) out
--As

--select @friends_fn = firstname, @friends_ln = lastname,@friends_em = email from
--(select MemberEmail from Friend  where Member2Email = @email
--union
--select Member2Email from Friend  where MemberEmail = @email) As emails
--inner join Member on Member.Email = emails.MemberEmail

--go

--declare @fn varchar(50), @ln varchar(50), @em varchar(50);
--execute GetFriend "ahmd", @friends_fn = @fn output, @friends_ln = @ln output, @friends_em = @em output;
--print @fn
--print @ln
--print @em

--declare @email varchar(50) = 'ahmdhsammfarge@gmail.com';
--select * from ( select * from Member where Email != @email ) As Emails 
--left join (select * from Friend Where MemberEmail = @email or Member2Email = @email) as UserFriends on Emails.Email = UserFriends.MemberEmail or Emails.Email = UserFriends.Member2Email
--left join (select * from FriendRequest Where MemberEmail = @email or Member2Email = @email) as UserRFriends on Emails.Email = UserRFriends.MemberEmail or Emails.Email = UserRFriends.Member2Email


--declare @name varchar(50) = 'house4';
--select rc.Name, avg(rating.value) from 
--(select * from ratingcriteria where RatingCriteria.PlaceName = @name) as rc
--left join rating on rc.Name = rating.criterianame and rc.placename = rating.placename
--group by rc.Name

--create proc DeletePlace (@name varchar(50))
--as
--begin
--delete from placephoto where placename = @name;
--delete from comment where placename = @name;
--delete from hashtag where placename = @name;
--delete from question where placename = @name;
--delete from visit where placename = @name;
--delete from rating where placename = @name;
--delete from ratingcriteria where placename = @name;
--delete from image where placename = @name;
--delete from invitation where placename = @name;
--delete from hotelfacility where hotelname = @name;
--delete from hotelroom where hotelname = @name;
--delete from museummonument where museumname = @name;
--delete from museumticket where museumname = @name;
--delete from hotel where name = @name;
--delete from museum where name = @name;
--delete from restaurant where name = @name;
--delete from city where name = @name;
--delete from place where name = @name;
--end

--create proc DeleteRatingCriteria @name varchar(50), @place varchar(50)
--as
--begin
--delete from Rating where PlaceName = @place and CriteriaName = @name;
--delete from RatingCriteria where PlaceName = @place and Name = @name;
--end