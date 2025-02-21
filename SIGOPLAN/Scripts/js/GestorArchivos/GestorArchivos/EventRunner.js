﻿// JavaScript source code
! function () {
    function e() {
        if (void 0 != window.pageYOffset) return {
            x: pageXOffset,
            y: t(pageYOffset)
        };
        var e, n, o = document,
			r = o.documentElement,
			i = o.body;
        return e = r.scrollLeft || i.scrollLeft || 0, n = r.scrollTop || i.scrollTop || 0, {
            x: e,
            y: t(n)
        }
    }

    function t(e) {
        return Number((e / (n() - window.innerHeight)).toFixed(2))
    }

    function n() {
        var e = document.body,
			t = document.documentElement,
			n = Math.max(e.scrollHeight, e.offsetHeight, t.clientHeight, t.scrollHeight, t.offsetHeight);
        return n
    }

    function o(e, t, n, o) {
        return e /= o / 2, e < 1 ? n / 2 * e * e + t : (e--, -n / 2 * (e * (e - 2) - 1) + t)
    }

    function r(e) {
        if (e) {
            for (var t = [], n = !1; null != e.parentNode;) {
                for (var o = 0, r = 0, i = 0; i < e.parentNode.childNodes.length; i++) {
                    var a = e.parentNode.childNodes[i];
                    a.nodeName == e.nodeName && (a === e && (r = o), o++)
                }
                var c = e.nodeName.toLowerCase();
                n && (c += "::shadow", n = !1), o > 1 ? t.unshift(c + ":nth-of-type(" + (r + 1) + ")") : t.unshift(c), e = e.parentNode, 11 === e.nodeType && (n = !0, e = e.host)
            }
            return t.splice(0, 1), t.join(" > ")
        }
    }

    function i(e) {
        N || (N = e);
        var t = e - N,
			n = o(t, E, b - E, X);
        if (window.scrollTo(0, n), t < X) {
            requestAnimationFrame(i)
        } else N = null, E = null, L = !1
    }

    function a() {
        c()
    }

    function c() {
        window.addEventListener("message", f, !0);
        var u;
        for (var e = 0; e < k.length; e++) window.addEventListener(k[e], u, !0);
        window.addEventListener("scroll", s, !0)
    }

    function s() {
        clearTimeout(w), w = setTimeout(function () {
            l()
        }, T)
    }

    function l() {
        C.postMessage(["event", {
            type: "scroll",
            positions: e()
        }], "*")
    }

    function u(e) {
        for (var t = r(e.target), n = e.constructor.name, o = e.type, i = {
            type: o,
            targetPath: t,
            eventClass: n
        }, a = {}, c = 0; c < x.length; c++) {
            var s = x[c];
            "undefined" != typeof e[s] && (a[s] = e[s])
        }
        i.options = a, Y.push(i), clearTimeout(g), g = setTimeout(function () {
            d()
        }, H)
    }

    function d() {
        C.postMessage(["event", Y], "*"), Y = []
    }

    function f(e) {
        var t = e.data;
        t.length && "event" === t[0] && t[1] && ("scroll" === t[1].type ? p(t[1].positions) : v(t[1]))
    }

    function p(e) {
        L !== !0 && (L = !0, E = document.documentElement.scrollTop || document.body.scrollTop, b = e.y * (n() - window.innerHeight), requestAnimationFrame(i))
    }

    function v(e) {
        for (var t = 0; t < e.length; t++) h(e[t])
    }

    function h(e) {
        "MouseEvent" === e.eventClass ? m(e) : "KeyboardEvent" === e.eventClass && y(e)
    }

    function m(e) {
        var t = document.querySelector(e.targetPath),
			n = new MouseEvent(e.type, {
			    bubbles: !0,
			    cancelable: !0,
			    view: window
			});
        t.dispatchEvent(n)
    }

    function y(e) {
        var t = document.querySelector(e.targetPath),
			n = new KeyboardEvent(e.type, e.options);
        t.dispatchEvent(n)
    }
    var w, g, E, b, N, T = 300,
		H = 500,
		Y = [],
		L = !1,
		X = 300,
		k = ["click", "keydown", "keypress", "keyup"],
		x = ["altKey", "code", "ctrlKey", "keyCode", "which", "clientX", "clientY", "layerX", "layerY", "offsetX", "offsetY", "pageX", "pageY", "screenX", "screenY", "x", "y"],
		C = window.parent;
    a()
}();