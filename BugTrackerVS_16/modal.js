(function () {
	function Modal(element, options) {
		this.options = $.extend({}, Modal.DEFAULT, options);
		this.$element = $(element);
		this.$bg = this.$element.find('.modal-bg');
		this.$content = this.$element.find('.modal-content');
		this.$close = this.$element.find('.modal-close');
		this.$body = $('body');
		this.contentMarginTop = this.$content.css('margin-top');
		this.contentStyle = this.$content.attr('style');
		this.init();
	}
	Modal.DEFAULT = {
		bgCancel: true,
		bgOpacity: .6,
		contentDuration: 250,
		bgDuration: 150,
		slideDistance: 150
	};
	Modal.prototype.init = function () {
		if (this.$body.hasClass('modal-open')) return false;
		else this.show();
		this.$close.click($.proxy(function () {
			if (this.$body.hasClass('modal-open')) this.hide();
			else return false;
		}, this));
		if (this.options.bgCancel) {
			this.$bg.click($.proxy(function () {
				this.hide();
			}, this));
		}
	};
	Modal.prototype.show = function () {
		var contentCss = {
			'margin-top': '-' + this.options.slideDistance + 'px'
		};
		if (this.$body.hasClass('modal-open')) return false;
		this.$bg.css('opacity', 0);
		this.$content.css(contentCss);
		this.$body.addClass('modal-open');
		this.$element.removeClass('modal-hidden');
		this.$bg.fadeTo(this.options.bgDuration, this.options.bgOpacity, $.proxy(function () {
			var self = this;
			this.$content.animate({
				'margin-top': this.contentMarginTop
			}, this.options.contentDuration, function () {
				self.recoverStyle();
			});
		}, this));
	};
	Modal.prototype.hide = function () {
		var self = this,
			contentAni = {
				'margin-top': '-' + this.options.slideDistance + 'px'
			};

		this.$content.animate(contentAni, this.options.contentDuration, function () {
			self.$bg.animate({
				opacity: 0
			}, self.options.bgDuration, function () {
				self.recoverStyle();
				self.$element.addClass('modal-hidden');
				self.$body.removeClass('modal-open');
			});
		});
	};
	Modal.prototype.recoverStyle = function () {
		if (this.contentStyle) this.$content.attr('style', this.contentStyle);
		else this.$content.removeAttr('style');
	};

	function Plugin(options) {
		return this.each(function () {
			var $this = $(this);
			if ($this.data('modal')) $this.data('modal').show();
			else $this.data('modal', new Modal(this, options));
		});
	}
	$.fn.modal = Plugin;
})();
