/**
 * @license Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	
	// Disable auto cleaning code when changing to view/source
	config.allowedContent = true;
	
	// Default setting.
	config.toolbarGroups = [
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection' ] },
		{ name: 'insert' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align' ] },
		{ name: 'links' },
		'/',
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'tools' },
		{ name: 'others' }
	];
	
	// FileBrowser
	config.filebrowserBrowseUrl = '/admin/fileManager/cke/filebrowse/';
	config.filebrowserImageBrowseUrl = '/admin/fileManager/cke/filebrowse/';
	config.filebrowserWindowWidth = '640';
    config.filebrowserWindowHeight = '480';
	config.filebrowserUploadUrl = '/admin/fileManager/cke/filebrowse/';
	config.filebrowserImageUploadUrl = '/admin/fileManager/cke/filebrowse/';
	config.filebrowserImageWindowWidth = '640';
    config.filebrowserImageWindowHeight = '480';
};

CKEDITOR.on( 'dialogDefinition', function( ev ) {
	// Take the dialog name and its definition from the event data.
	var dialogName = ev.data.name;
	var dialogDefinition = ev.data.definition;

	// Check if the definition is from the dialog we're
	// interested on (the Link dialog).
	if ( dialogName == 'link' )
	{
		// FCKConfig.DefaultLinkTarget = '_blank'
		// Get a reference to the "Target" tab.
		var targetTab = dialogDefinition.getContents( 'target' );
		// Set the default value for the URL field.
		var targetField = targetTab.get( 'linkTargetType' );
		targetField[ 'default' ] = '_blank';
	}
});