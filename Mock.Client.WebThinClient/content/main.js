"use strict";

(function ($) {
    var endpointUri,
        resultDiv,
        timeout = 2000,

        btnClean_onClick = function () {
            $(resultDiv).html('<div id="result_other">\n\t<h4>Splošne napake</h4>\n\t<pre class="log"></pre>\n</div>');
        },

        btnSubmit_onClick = function () {
            $.post(endpointUri + "/start", {})
            .done(function (data) {
                if (0 != data.Status) {
                    var id = data.JobId;
                    $('<div id="result_' + id + '" class="result">\n\t<h4 title="Klikni za podrobnosti" alt="Klikni za podrobnosti">' +
                        '<i></i> Simulacija (' + id + ')</h4>\n\t<pre class="log hidden"></pre></div>')
                        .insertBefore("#result_other");
                    $('#result_' + id + ' h4').click(function () {
                        var pre = $('#result_' + id + ' pre');
                        if (pre.hasClass('hidden')) { pre.removeClass('hidden'); }
                        else { pre.addClass('hidden'); }
                    });
                    updateStatus(id, "Start @ node: " + data.Node, data.Status);
                    setTimeout(function () {
                        checkJobStatus(id)
                    }, timeout);
                }
                else {
                    updateStatus(null, "Napaka pri klicu API-ja: " + data.Error, 0);
                }
            })
            .fail(function (data) {
                updateStatus(null, "Napaka pri dostopu do API-ja na naslovu: " + endpointUri + ". ", 0);
            });
        },

        checkJobStatus = function (id) {
            $.get(endpointUri + "/query/" + id, {})
            .done(function (data) {
                if ((0 != data.Status) && (3 != data.Status)) {
                    var id = data.JobId;
                    updateStatus(id, "Se obdeluje...", data.Status);
                    setTimeout(function () {
                        checkJobStatus(id)
                    }, timeout);
                }
                else if (3 == data.Status) {
                    var id = data.JobId;
                    updateStatus(id, "Konec...", data.Status);
                }
                else {
                    updateStatus(null, "Napaka pri klicu API-ja: " + data.Error, 0);
                }
            })
            .fail(function (data) {
                updateStatus(null, "Napaka pri dostopu do API-ja na naslovu: " + endpointUri + ". ", 0);
            });
        },

        updateStatus = function (id, message, status) {
            var newDate = new Date();
            if (!id) id = "other";
            var result = $("#result_" + id);
            if (id !== "other") {
                if (2 == status) {
                    $("h4 i", result).get(0).className = "fa fa-circle-o-notch fa-spin status" + status;
                }
                else {
                    $("h4 i", result).get(0).className = "fa fa-circle status" + status;
                }
                $("h4 i", result).attr('title', convertStatusToString(status));
                $("h4 i", result).attr('alt', convertStatusToString(status));
            }
            $("pre", result).get(0).innerHTML += newDate.toLocaleDateString() + "@" + newDate.toLocaleTimeString() + ": " + message + "\n";
        },

        convertStatusToString = function (status) {
            switch (status) {
                case 0:
                    return "Error";
                case 1:
                    return "Pending";
                case 2:
                    return "In progress";
                case 3:
                    return "Done";
                default:
                    return "Unknown";
            }
        };


    $(document).ready(function () {
        resultDiv = document.getElementById("divResult");
        btnClean_onClick();
        var btnSubmit = document.getElementById("btnSubmit");
        if (btnSubmit) {
            $(btnSubmit).click(function (e) {
                e.preventDefault();
                endpointUri = document.getElementById("tbUrlProxy").value;
                btnSubmit_onClick();
            });
        }
        var btnClean = document.getElementById("btnClean");
        if (btnClean) {
            $(btnClean).click(function (e) {
                e.preventDefault();
                btnClean_onClick();
            });
        }
    });
})(jQuery);