﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DemoStoreInstaller.ascx.cs" Inherits="UCommerce.RazorStore.Installer.umbraco.uCommerce.Install.DemoStoreInstaller1" %>
<div style="width:100%;background-color:#231f21;">
    <img src="../images/uCommerce-logo-small.png" style="margin:10px;" />
</div>
<asp:Panel runat="server" ID="pnlInstall">
    <h1>Installation Complete</h1>
    <h2>What's installed?</h2>
    <ul>
        <li>Example Document Types</li>
        <li>Example Templates (master pages)</li>
        <li>An example site structure</li>
        <li>A set of Scripting Files to display the catalogue</li>
    </ul>
    <h2>What next?</h2>
    <p><asp:CheckBox runat="server" ID="chkSettings" Text="Install the example uCommerce configuration"/> <a href="#configuration" class="more-info">[?]</a></p>
    <div id="configuration" style="display:none;" class="more-info-div">
        <h3>What's included?</h3>
        <ul>
            <li>Example custom Data Type "Colour"</li>
            <li>Example Product Definitions for: Shirt, Shoe and Accessory</li>
        </ul>
    </div>
    <p><asp:CheckBox runat="server" ID="chkCatalog" Text="Install the example catalogue" /> <a href="#catalogue" class="more-info">[?]</a></p>
    <div id="catalogue" style="display:none;" class="more-info-div">
        <h3>What's included?</h3>
        <ul>
            <li>Your own personal version of the example uCommerce store <a href="http://avenue-clothing.com" target="_blank">avenue-clothing.com</a></li>
            <li>Example category structure:
                <ul>
                    <li>Shirts
                        <ul>
                            <li>Formal Shirts</li>
                            <li>Casual Shirts</li>
                        </ul>
                    </li>
                    <li>Shoes</li>
                    <li>Accessories
                        <ul>
                            <li>Scarves</li>
                            <li>Belts</li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
    <p><asp:Button runat="server" ID="btnInstall" Text="Install" OnClick="btnInstall_Click"/></p>
</asp:Panel>
<asp:Panel runat="server" ID="pnlthanks" Visible="false">
    <h1>Catalogue Added</h1>
    <p>Thanks. You're all done and now have a demo store up and running.</p>
    <p><asp:Button runat="server" ID="btnAssignPermissions" Text="Assign Permissions" OnClick="btnAssignPermissions_Click"/></p>
</asp:Panel>
<div class="well sidebar-nav" id="newsletter">
    <h2>Keep up-to-date</h2>
    <p>Sign up to our newsletter.</p>
    <div id="newsletter-form">
        <label for="name">Name:</label><br /><input type="text" name="cm-name" id="name" /><br />
        <label for="xqlur-xqlur">Email:</label><br /><input type="text" name="cm-xqlur-xqlur" id="xqlur-xqlur" class="required email" /><br />
        <input type="checkbox" name="cm-ol-xbjtk" id="AvenueClothingSignup" /><label for="AvenueClothingSignup">Also sign up for uCommerce retailer news</label><br />
        <button type="submit" class="btn">Go</button>
	</div>
</div>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js"></script>
<script>
    $(function () {
        var info = $('#configuration,#catalogue');
        var close = $('<a />', {
            text:"[×]",
            href:"#",
            click:function(e){
                e.preventDefault();
                $(this).parents('div.more-info-div').slideUp();
                return false;
            }
        });
        close.appendTo($('h3', info));
        info.hide();
        $('.more-info').click(function (e) {
            e.preventDefault();
            var t = $(this);
            var id = t.attr("href");
            $(id).slideDown();
            return false;
        });
        $('#newsletter-form button').click(function (e) {
            e.preventDefault();
            var form = $('#newsletter-form');
            $.getJSON("http://thesitedoctor.createsend.com/t/y/s/xqlur/?callback=?", $('input', form).serialize(), function (data) {
                console.log(data);
                var thanks = $('<p />', { text: "Thanks! We\'ll be in touch soon!" });
                if (data.Status === 400) {
                    alert("Error: " + data.Message);
                    form.css('opacity', 1);
                    $('input:first', form).focus();
                } else { // 200
                    form.fadeOut(300, function () { $(this).css('opacity', 1).html(thanks).fadeIn(300); });
                }
            });
            return false;
        });
    });
</script>