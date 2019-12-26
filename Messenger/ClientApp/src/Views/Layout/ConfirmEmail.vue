<template>

</template>

<script>
    export default {
        name: "ConfirmEmail",
        created: function() {
            var token = this.$route.query.token;
            this.$store.dispatch("account/ConfirmRegistration",token,{root: true})
              .then(()=>this.emailConfirmed());
        },
        methods:{
            emailConfirmed: function(){
                this.$router.push('/login');
            }
        }
    }
</script>

<style scoped>

</style>
