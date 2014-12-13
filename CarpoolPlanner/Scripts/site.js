$.fn.extend({
    // Disables a control and clears its value. The value is restored when the control is re-enabled.
    disableAndClear: function(disable) {
        if (disable == undefined)
            disable = true;
        if (disable == this.prop('disabled'))
            return;
        this.prop('disabled', disable);
        if (disable) {
            this.attr('hiddenvalue', this.val());
            this.val('');
        } else {
            var hidden = this.attr('hiddenvalue');
            if (hidden != undefined) {
                this.val(hidden);
                this.removeAttr('hiddenvalue');
            }
        }
    },
    // Enables/disables control(s) idendified by the selector based on the checked state of the target
    checkedEnabledControl: function(selector) {
        var target = this;
        var checked = target.prop('checked');
        if (checked != undefined) {
            var controls = $(selector);
            controls.prop('disabled', !checked);
            target.change(function() {
                checked = target.prop('checked');
                controls.prop('disabled', !checked);
            });
        }
    },
    // Enables/disables control(s) idendified by the selector based on the checked state of the target
    uncheckedEnabledControl: function(selector) {
        var target = this;
        var checked = target.prop('checked');
        if (checked != undefined) {
            var controls = $(selector);
            controls.prop('disabled', checked);
            target.change(function() {
                checked = target.prop('checked');
                controls.prop('disabled', checked);
            });
        }
    }
});

function registerPageLoad(fn) {
    if (window.Sys) {
        Sys.Application.add_load(fn);
    } else {
        $(document).ready(fn);
    }
}