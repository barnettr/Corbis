/****************************************************
    Corbis UI Watermark
***************************************************/

CorbisUI.Watermark = {
    initialize: function() {
        $$('.Watermarked').each(function(el) {
            if (el.value.trim() == "") {
                el.value = el.title;
                el.setProperty('class', el.getProperty('class') + " " + el.getProperty('watermarkcss'));
            }
            el.addEvent("click", function() {
                if (this.value == this.title) {
                    this.value = "";
                    this.setProperty('class', this.getProperty('class').replace(this.getProperty('watermarkcss'), ""));
                }
            });
            el.addEvent("focus", function() {
                if (this.value == this.title) {
                    this.value = "";
                    this.setProperty('class', this.getProperty('class').replace(this.getProperty('watermarkcss'), ""));
                }
            });
            el.addEvent("blur", function() {
                if (this.value.trim() == "") {
                    this.value = this.title;
                    this.setProperty('class', this.getProperty('class') + " " + this.getProperty('watermarkcss'));
                }
            });
        });
    },

    ChangeValue: function(el) {
        try {
            if (el.value.trim() == "") {
                el.value = el.title;
                el.setProperty('class', el.getProperty('class') + " " + el.getProperty('watermarkcss'));
            } else {
                el.setProperty('class', el.getProperty('class').replace(el.getProperty('watermarkcss'), ""));
            }
        } catch (exception) {

        }
    }
};