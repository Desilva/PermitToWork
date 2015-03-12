/**
 * widget for owner grid on owner page frontend
 */
;(function($, window, document, undefined){
    $.widget('ww.workflowDiagram', {
        options: {
            title: '',
            template: '',
            url: '',
            //ongoingColor: '#00FFFF ', //cyan
            //finishColor: '#00FF00', //lime
            //emptyColor: '#FFFFFF',
            cancellationSelector: '.workflow-cancellation',
        },

        //attributes
        dataSource: null,
		
        _create: function(){
            var self = this;

            //workflow diagram template
            //var template = kendo.template('' +
            //    'div class="position-relative"' +
            //        '<div class="position-absolute font-size" style="">Initiator</div>' +
		    //        '<div class="position-absolute"  style="" id="boxInitiator">' +
            //            self.options.svgBox  +
            //        '</div>' +
            //        '<div class="positionAbsolute" style="">' +
            //        '</div>' +
            //    '</div>'
            //);

            //window
            self.element.kendoWindow({
                title: self.options.title,
                width: 1500,
                height: 500,
                visible: false,
                modal: true,
                content: {
                    //url: self.options.bindingUrl,
                    //dataType: 'json',
                    template: self.options.template,
                }
            });
        },
		
        _init: function(){
        },
		
        _destroy: function(){
            $.Widget.prototype.destroy.call(this);
        },

        //menampilkan popup window berisi gambar workflow
        show: function () {
            var self = this;

            self.element.data('kendoWindow').center().open();
        },

        //reload data untuk gambar workflow
        //kalau tidak ada cancellation node dari server (NodeName = CANCELLATION_*), hide $(cancellationSelector)
        //mengganti title
        reload: function (id, num) {
            //kamus
            var self = this;
            var status, selector;
            var isCancellation = false;

            //algoritma
            self.element.data('kendoWindow').title(num);
            $.post(self.options.url + '?id=' + id, function (data) {
                for (var i in data) {
                    status = data[i].StatusString;
                    //selector = '#' + data[i].NodeName + ' .workflow-box';
                    selector = 'div[data-node-name=' + data[i].NodeName + '] .workflow-box';

                    if (status === 'ONGOING') {
                        self.element.find(selector).removeClass('workflow-empty');
                        self.element.find(selector).addClass('workflow-ongoing');
                        self.element.find(selector).removeClass('workflow-finish');
                        //self.element.find(selector).css('background-color', self.options.ongoingColor);
                    } else if (status === 'FINISH') {
                        self.element.find(selector).removeClass('workflow-empty');
                        self.element.find(selector).removeClass('workflow-ongoing');
                        self.element.find(selector).addClass('workflow-finish');
                        //self.element.find(selector).css('background-color', self.options.finishColor);
                    } else { //empty
                        self.element.find(selector).addClass('workflow-empty');
                        self.element.find(selector).removeClass('workflow-ongoing');
                        self.element.find(selector).removeClass('workflow-finish');
                        //self.element.find(selector).css('background-color', self.options.emptyColor);
                    }

                    if (data[i].NodeName.indexOf('CANCELLATION') > -1)
                        isCancellation = true;
                }

                if (isCancellation) {
                    self.element.find(self.options.cancellationSelector).show();
                } else {
                    self.element.find(self.options.cancellationSelector).hide();
                }
            }, 'json');
        },
	});
})(jQuery, window, document);