$(document).ready(function(){
    $('.editbtn').click(function(){
        $(this).html($(this).html() == 'edit' ? 'modify' : 'edit');
    });
});