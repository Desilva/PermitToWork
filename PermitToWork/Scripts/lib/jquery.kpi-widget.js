/**
 * widget for owner grid on owner page frontend
 */
;(function($, window, document, undefined){
    $.widget('ww.kpiWidget', {
        options: {
            url: '',
        },
		
        _create: function(){
            var self = this;

            self.element.find('form').submit(function (e) {
                e.preventDefault();

                self._reloadKpiData($(this).serializeArray());
            }).submit();
        },
		
        _init: function(){
        },

        _reloadKpiData: function (data) {
            var self = this;

            $.post(self.options.url, data, function (response) {
                self.element.find('div[data-kpi-type=requestor-ontime]').html(response.RequestorOntimeClosing);
                self.element.find('div[data-kpi-type=requestor-overdue]').html(response.RequestorOverdueClosing);
            }, 'json')
        },
		
        _destroy: function(){
            $.Widget.prototype.destroy.call(this);
        },
	});
})(jQuery, window, document);