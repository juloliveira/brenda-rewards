"use strict";

// Class Definition
var KTLogin = function() {
    var _login;

    var _showForm = function(form) {
        var cls = 'login-' + form + '-on';
        var form = 'kt_login_' + form + '_form';

        _login.removeClass('login-forgot-on');
        _login.removeClass('login-signin-on');
        _login.removeClass('login-signup-on');
        _login.removeClass('login-created-on');

        _login.addClass(cls);

        KTUtil.animateClass(KTUtil.getById(form), 'animate__animated animate__backInUp');
    }

    $('input[name=companydocument]').mask('00.000.000/0000-00', {
        reverse: true
    });
    

    var _handleSignInForm = function() {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			KTUtil.getById('kt_login_signin_form'),
			{
				fields: {
					username: {
						validators: {
							notEmpty: {
                                message: 'É necessário preencher o campo e-mail'
                            },
                            emailAddress: {
                                message: 'O e-mail digitado não é válido'
                            }
						}
					},
					password: {
						validators: {
							notEmpty: {
                                message: 'É necessário preencher o campo senha'
							}
						}
					}
				},
				plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

        $('#kt_login_signin_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function(status) {
		        if (status != 'Valid') {
					swal.fire({
		                text: "Foram encontrados erros no formulário.",
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: "Fechar",
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle forgot button
        $('#kt_login_forgot').on('click', function (e) {
            e.preventDefault();
            _showForm('forgot');
        });

        // Handle signup
        $('#kt_login_signup').on('click', function (e) {
            e.preventDefault();
            _showForm('signup');
        });
    }

    var _handleSignUpForm = function(e) {
        var validation;
        var form = KTUtil.getById('kt_login_signup_form');

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			form,
			{
				fields: {
                    companyname: {
                        validators: {
                            notEmpty: {
                                message: 'É necessário preencher o campo Empresa'
                            }
                        }
                    },
                    companydocument: {
                        validators: {
                            notEmpty: {
                                message: 'É necessário preencher o campo Empresa'
                            }
                        }
                    },
					name: {
						validators: {
							notEmpty: {
								message: 'É necessário preencher o campo Nome'
							}
						}
					},
					email: {
                        validators: {
							notEmpty: {
                                message: 'É necessário preencher o campo e-mail'
							},
                            emailAddress: {
								message: 'O e-mail digitado não é válido'
							}
						}
					},
                    password: {
                        validators: {
                            notEmpty: {
                                message: 'É necessário preencher o campo senha'
                            }
                        }
                    },
                    confirmpassword: {
                        validators: {
                            notEmpty: {
                                message: 'É necessário preencher o campo de confirmação de senha'
                            },
                            identical: {
                                compare: function() {
                                    return form.querySelector('[name="password"]').value;
                                },
                                message: 'A senha e a confirmação de senha devem ser iguais'
                            }
                        }
                    },
                    agree: {
                        validators: {
                            notEmpty: {
                                message: 'Você precisa concordar com os termos e condições de uso.'
                            }
                        }
                    },
				},
				plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap(),
				}
			}
		);

        $('#kt_login_signup_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function(status) {
		        if (status == 'Valid') {
                    grecaptcha.ready(function () {
                        grecaptcha.execute('6LctLMsZAAAAAGIovAgjvWXdEK-VJkyEb6Sy-d4C', { action: 'signup' }).then(function (token) {
                            var user = {
                                companyName: $('#kt_login_signup_form input[name=companyname]').val(),
                                companyDocument: $('#kt_login_signup_form input[name=companydocument]').val(),
                                name: $('#kt_login_signup_form input[name=name]').val(),
                                email: $('#kt_login_signup_form input[name=email]').val(),
                                password: $('#kt_login_signup_form input[name=password]').val(),
                                confirmPassword: $('#kt_login_signup_form input[name=confirmpassword]').val(),
                                token: token,
                                __RequestVerificationToken: $('#kt_login_signup_form input[name=__RequestVerificationToken]').val()
                            }
                            $.ajax({
                                url: '/security/sign-up',
                                method: 'post',
                                data: user,
                                success: function (e) {
                                    if (e.result == 'successful') {
                                        _showForm('created');
                                    } else {
                                        var errors = "";
                                        $(e.errors).each(function (idx, item) {
                                            errors += item + "<br/>";
                                        });

                                        swal.fire({
                                            title: "Encontramos os seguintes erros:",
                                            html: errors,
                                            icon: "error",
                                            buttonsStyling: false,
                                            confirmButtonText: "Fechar",
                                            customClass: {
                                                confirmButton: "btn font-weight-bold btn-light-primary"
                                            }
                                        }).then(function () {
                                            KTUtil.scrollTop();
                                        });
                                    }
                                },
                                error: function (e) {
                                    
                                    swal.fire({
                                        text: "Não foi possível criar seu usuário. Tente novamente!",
                                        icon: "error",
                                        buttonsStyling: false,
                                        confirmButtonText: "Fechar",
                                        customClass: {
                                            confirmButton: "btn font-weight-bold btn-light-primary"
                                        }
                                    }).then(function () {
                                        KTUtil.scrollTop();
                                    });
                                }
                            });
                        });
                    });
				} else {
					swal.fire({
                        text: "Foram encontrados erros no formulário.",
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: "Fechar",
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle cancel button
        $('#kt_login_signup_cancel').on('click', function (e) {
            e.preventDefault();

            _showForm('signin');
            
        });
    }

    var _handleForgotForm = function(e) {
        var validation;

        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
			KTUtil.getById('kt_login_forgot_form'),
			{
				fields: {
					email: {
						validators: {
							notEmpty: {
                                message: 'É necessário preencher o campo e-mail'
							},
                            emailAddress: {
                                message: 'O e-mail digitado não é válido'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					bootstrap: new FormValidation.plugins.Bootstrap()
				}
			}
		);

        // Handle submit button
        $('#kt_login_forgot_submit').on('click', function (e) {
            e.preventDefault();

            validation.validate().then(function(status) {
		        if (status == 'Valid') {
                    // Submit form
                    KTUtil.scrollTop();
				} else {
					swal.fire({
		                text: "Sorry, looks like there are some errors detected, please try again.",
		                icon: "error",
		                buttonsStyling: false,
		                confirmButtonText: "Ok, got it!",
                        customClass: {
    						confirmButton: "btn font-weight-bold btn-light-primary"
    					}
		            }).then(function() {
						KTUtil.scrollTop();
					});
				}
		    });
        });

        // Handle cancel button
        $('#kt_login_forgot_cancel').on('click', function (e) {
            e.preventDefault();

            _showForm('created');
        });
    }

    // Public Functions
    return {
        // public functions
        init: function() {
            _login = $('#kt_login');

            _handleSignInForm();
            _handleSignUpForm();
            _handleForgotForm();
        }
    };
}();

// Class Initialization
jQuery(document).ready(function() {
    KTLogin.init();
});
