﻿! function () {
    function n() {
        l(), t()
    }

    function t() {
        window.addEventListener("message", a, !0)
    }

    function e(n) {
        var t;
        try {
            t = {}.toString.call(n)
        } catch (n) {
            t = "[object Object]"
        }
        return t
    }

    function o(n, t) {
        for (var e = n.length; e--;)
            if (n[e] === t) return !0;
        return !1
    }

    function r(n) {
        return !!(n && "object" == typeof n && "nodeType" in n && 1 === n.nodeType && n.outerHTML)
    }

    function c(n, t) {
        return n.toLowerCase() < t.toLowerCase() ? -1 : 1
    }

    function i(n) {
        if (null === n || "undefined" == typeof n) return 1;
        var t, o = e(n);
        if ("[object Number]" === o || "[object Boolean]" === o || "[object String]" === o) return 1;
        if ("[object Function]" === o || "[object global]" === o) return 2;
        if ("[object Object]" === o) {
            var r = Object.keys(n);
            for (t = 0; t < r.length; t++) {
                var c = n[r[t]];
                if (i = {}.toString.call(c), "[object Function]" === i || "[object Object]" === i || "[object Array]" === i) return 2
            }
            return 1
        }
        if ("[object Array]" === o) {
            for (t = 0; t < n.length; t++) {
                var c = n[t],
                    i = {}.toString.call(c);
                if ("[object Function]" === i || "[object Object]" === i || "[object Array]" === i) return 2
            }
            return 1
        }
        return 2
    }

    function u(n, t, o) {
        var r, i, l = "",
            a = [];
        if (o = o || "", t = t || [], null === n) return "null";
        if ("undefined" == typeof n) return "undefined";
        if (l = e(n), "[object Object]" == l && (l = "Object"), "[object Number]" == l) return "" + n;
        if ("[object Boolean]" == l) return n ? "true" : "false";
        if ("[object Function]" == l) return n.toString().split("\n  ").join("\n" + o);
        if ("[object String]" == l) return '"' + n.replace(/"/g, "'") + '"';
        for (i = 0; i < t.length; i++)
            if (n === t[i]) return "[circular " + l.slice(1) + ("outerHTML" in n ? " :\n" + n.outerHTML.split("\n").join("\n" + o) : "");
        if (t.push(n), "[object Array]" == l) {
            for (r = 0; r < n.length; r++) a.push(u(n[r], t));
            return "[" + a.join(", ") + "]"
        }
        if (l.match(/Array/)) return l;
        var f = l + " ",
            s = o + "  ";
        if (o.length / 2 < 2) {
            var b = [];
            try {
                for (r in n) b.push(r)
            } catch (n) { }
            for (b.sort(c), r = 0; r < b.length; r++) try {
                a.push(s + b[r] + ": " + u(n[b[r]], t, s))
            } catch (n) { }
        }
        return a.length ? f + "{\n" + a.join(",\n") + "\n" + o + "}" : f + "{}"
    }

    function l() {
        if (window.console)
            for (var n = 0; n < f.length; n++) ! function () {
                var t = f[n];
                window.console[t] && (window.console[t] = function () {
                    for (var n = [].slice.call(arguments), e = [], o = [], c = 0; c < n.length; c++) r(n[c]) ? (o.push(u(n[c].outerHTML)), e.push(1)) : (o.push(u(n[c])), e.push(i(n[c])));
                    b.postMessage(["console", {
                        "function": t,
                        arguments: o,
                        complexity: Math.max.apply(null, e)
                    }], "*"), this.apply(console, n)
                }.bind(console[t]))
            }()
    }

    function a(n) {
        var t = n.data;
        if ("object" == typeof t && "command" === t.type) {
            try {
                var e = window.eval(t.command)
            } catch (n) {
                return void console.error(n.message)
            }
            if (.30000000000000004 === e) return void console.log("I love JavaScript too.");
            if (o(s, t.command)) return void console.log("Plz no WATS.");
            console.log(e)
        }
    }
    var f = ["log", "error", "warn", "info", "debug", "table", "time", "timeEnd", "count", "clear"],
        s = ["({} + [])", "({} + []);", "({} + [])", "({} + []);", "{} + {}", "{} + {};", "({} + {})", "({} + {});", "[] == []", "[] == [];", "[] == ![]", "[] == ![];", "[] + []", "[] + [];"],
        b = window.parent;
    n()
}();