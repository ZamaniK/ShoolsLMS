


$("#CRUDModal").on('hidden.bs.modal', function (e) {
    var modal = $(this);
    modal.removeData('bs.modal');
    modal.find('.modal-body').html('');
    modal.find('.btnsubmit').off('click');
    modal.find('.btnsubmit').show();

});

$('#CRUDModal').on('show.bs.modal', function (event) {

    var modal = $(this);
    var button = $(event.relatedTarget);
    var href = button.attr('href');
    var modalContent = modal.find('.modal-body');
    modal.find('.progress').hide();
    modalContent.load(href, function (response, status, xhr) {
        if (status == "error") {
            modal.find('.btnsubmit').hide();
            var ModalEmpty = '<h4 class="text-warning">لايمكن اظهار الصفحةّ</h4>';
            modalContent.html(ModalEmpty);
        } else {
            modal.find('.btnsubmit').on('click', function (e) {
                var tt = modal.find('form');
                modal.find('form').submit();
            });
        }
    });

    //modal.modal('handleUpdate')


});

$('#CRUDModal').on('shown.bs.modal', function (event) {

    var modal = $(this);
    var button = $(event.relatedTarget);
    var ViewParent = button.closest('.Viewparent');
    var ViewParentUrl = button.data('bind');
    var Progress = modal.find('.progress');
    var modalContent = modal.find('.modal-body');


    //modal.modal('handleUpdate')



    modal.find('form').submit(function (e) {
        Progress.show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (ViewParentUrl == "") {
                    if (result.success) {
                        Progress.hide();
                        modal.modal('hide');
                        location.reload();


                    } else {
                        Progress.hide();
                        modalContent.html(result);
                        //bindForm();
                    }
                    //return false;
                }
                if (result.success) {
                    Progress.hide();
                    modal.modal('hide');
                    ViewParent.load(ViewParentUrl);


                } else {
                    Progress.hide();
                    modalContent.html(result);

                }
            }
        });
        return false;
    });
});

