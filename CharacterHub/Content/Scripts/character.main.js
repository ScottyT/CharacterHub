if (typeof character === "undefined") {
	var character = {};
}

character.main = (function ($) {
	var main = {
		init: function () { 
			this.sortList();
			this.starRating();
			this.notifications();
		},

		sortList: function () {
			var array = $('.character-item');
			array.each(function (i, value) {
				var attribute = value;
				console.log("Inital order: " + i + ", Id:" + $(this).attr("id"));
			});
			
			$(".characters").sortable({
				update: function(event, ui) {
					var order = $('.characters').sortable('toArray');

					array = $('.character-item');
					$.ajax({
						type: "POST",
						url: "/api/order",
						dataType: "json",
						data: JSON.stringify({ "Id": order.Id, "CharacterId": order.CharacterId }),
						success: function(result) {
							console.log(result);
						}
					});

					
				}
			}).disableSelection();
			$('#sort-submit').on('click', function() {
				var sorted = $('.characters').sortable("serialize", { attribute: "id" });
				console.log(sorted);
			});
			return array;
		},

		starRating: function () {
			$('.star-wrapper .star').on('mouseover', function () {
				var onStar = parseInt($(this).data('value'), 10);
				var starRating = $(this).data('rating');
				$(this).parent().children('.star').each(function (e) {
					var ratingDesc = $(this).closest('.star-wrapper').find('.rating-desc p');					
					if (e < onStar) {
						$(this).addClass('hover');
						ratingDesc.html(starRating);
					} else {
						$(this).removeClass('hover');
					}
				});
			}).on('mouseout', function () {			
				$(this).parent().children('.star').each(function (e) {
					$(this).removeClass('hover');				
					var ratingDesc = $(this).closest('.star-wrapper').find('.rating-desc p');
					var origstarRating = ratingDesc.data('orig-rating');
					if (typeof origstarRating !== "undefined") {
						ratingDesc.html(origstarRating);
					} else {
						ratingDesc.text("Not Rated");
					}
					
				});
			});

			// Gets characters
			$.ajax({
				type: "GET",
				url: "/api/characters",
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (characters) {
					$.each(characters, function(i) {
						var ratingNumber = characters[i].RatingNumber * 2;
						var charId = characters[i].CharacterId;

						//Finding the character
						var $starWrapper = $('.characters').find(`[data-id='${charId}']`).next('.star-wrapper');
						var $stars = $starWrapper.find('.star');

						// Add classes
						$stars.each(function(e) {
							if (e < ratingNumber) {
								$(this).addClass('selected');
							}
						});
					});
				}
			});

			$('.star-wrapper .star').on('click', function () {
				var $this = $(this);
				var number = $this.closest(".character-item").find('.name').data("id"); 
				var starRating = $this.data('number');
				var ratingTitle = $this.data('rating');
				var name = $this.closest('.character-item').find('.name p').html().trim();
				var ratingDesc = $this.closest('.star-wrapper').find('.rating-desc p');
				var $ratingWrapper = $this.closest('.character-item').find('.rating-number');
				var $averageRating = $this.closest('.character-item').find('.average-rating span');

				$.ajax({
					type: "POST",
					url: "/api/rating",
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					data: JSON.stringify({ "RatingNumber": starRating, "RatingTitle": ratingTitle, "Id": number, "CharacterName": name }),
					success: function (result) {
						var newRatingTitle = result.characters.RatingTitle;
						ratingDesc.data('orig-rating', newRatingTitle);
						ratingDesc.html(newRatingTitle);
						var ratingNumber = result.characters.RatingNumber;
						$ratingWrapper.html(ratingNumber);
						var averagerating = result.characters.AverageRating;
						$averageRating.html(averagerating);
						if (!result.messages.IsSuccess) {
							alert(result.messages.Message);  
						}
					}
				});
				
				var onStar = parseInt($(this).data('value'));
				$(this).parent().children('.star').each(function (e) {
					var ratingDesc = $(this).parent().find('.rating-desc');
					if (e < onStar) {
						$(this).addClass('selected');
						
					} else {
						$(this).removeClass('selected');
					}
				});
			});


		},

		notifications: function () {
			$('.dropdown-menu .notification').on('click', function() {
				var $this = $(this);
				var notificationId = $this.data('id');
				var characterId = $this.data('character-id');
				$.ajax({
					type: "POST",
					url: "/api/notification",
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					data: JSON.stringify({ "NotificationId": notificationId, "CharacterId": characterId, "IsRead": true }),
					success: function(result) {
						
					}
				});
			});
			var notifyLen = $('.notify-center .dropdown-menu .notification').length;
			if (notifyLen > 0) {
				$('.notify-center').addClass('has-notifications');
				$('.notify-center').append("<span class='alert-icon'><p>" + notifyLen + "</p></span>");
			}
		}
	};
	return main; 
})(jQuery);   

$(function () {  
	character.main.init();   
});     