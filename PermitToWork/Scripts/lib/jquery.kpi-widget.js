/**
 * widget for owner grid on owner page frontend
 */
;(function($, window, document, undefined){
    $.widget('ww.kpiWidget', {
        options: {
            url: '',
            downloadUrl: '',
        },
		
        _create: function(){
            var self = this;
            var form = self.element.find('form');

            //form.submit(function (e) {
            //    e.preventDefault();
            //});

            self.element.find('#filter-report').click(function () {
                self._reloadKpiData(form.serializeArray());
            });

            self.element.find('#download-report').click(function () {
                self._downloadKpiData(form.serialize());
            });

            self._reloadKpiData(form.serializeArray());
        },
		
        _init: function(){
        },

        _reloadKpiData: function (arrayFormData) {
            var self = this;

            self._startReload();
            $.post(self.options.url, arrayFormData, function (response) {
                self.element.find('div[data-kpi-type=requestor-ontime]').html(response.RequestorOntimeClosing);
                self.element.find('div[data-kpi-type=requestor-overdue]').html(response.RequestorOverdueClosing);
                self.element.find('div[data-kpi-type=supervisor-response-time]').html(kendo.toString(response.SupervisorAverageResponseTime, 'n2') + ' hours');
                self.element.find('div[data-kpi-type=assessor-response-time]').html(kendo.toString(response.AssessorAverageResponseTime, 'n2') + ' hours');
                self.element.find('div[data-kpi-type=fo-closing-permit]').html(response.FOClosingApprove);
                self.element.find('div[data-kpi-type=fo-closing-time]').html(kendo.toString(response.FOAverageClosingTime, 'n2') + ' hours');

                self._finishReload();
            }, 'json')
        },

        _downloadKpiData: function (stringFormData) {
            var self = this;
            var url = self.options.downloadUrl + '?' + stringFormData;
            var win = window.open(url, '_blank');
            win.focus();
        },

        _startReload: function () {
            var self = this;

            self.element.find('.loading').show();
            self.element.find('#filter-report').attr('disabled', 'disabled');
        },

        _finishReload: function () {
            var self = this;

            self.element.find('.loading').hide();
            self.element.find('#filter-report').removeAttr('disabled');

        },
		
        _destroy: function(){
            $.Widget.prototype.destroy.call(this);
        },
	});
})(jQuery, window, document);