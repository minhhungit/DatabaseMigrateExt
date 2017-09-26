using System;
using DatabaseMigrateExt.Attributes;
using DatabaseMigrateExt.Models;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMigration(DatabaseScriptType.SqlDataAndStructure, 2017, 10, 21, 19, 40, 01)]
    public class MovieStore_20171021_194001_inital_tables : Migration
    {
        public override void Up()
        {
            if (!this.Schema.Schema("dbo").Table("Movie").Exists())
            {
                Create.Table("Movie").InSchema("dbo")
                    .WithColumn("MovieId").AsInt32().Identity().PrimaryKey().NotNullable()
                    .WithColumn("Title").AsString(200).NotNullable()
                    .WithColumn("Description").AsString(1000).Nullable()
                    .WithColumn("Storyline").AsString(Int32.MaxValue).Nullable()
                    .WithColumn("Year").AsInt32().Nullable()
                    .WithColumn("ReleaseDate").AsDateTime().Nullable()
                    .WithColumn("Runtime").AsInt32().Nullable();

                Insert.IntoTable("Movie").InSchema("dbo")
                .Row(new
                {
                    Title = "The Matrix",
                    Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    Storyline = "Thomas A. Anderson is a man living two lives. By day he is an average computer programmer and by night a hacker known as Neo. Neo has always questioned his reality, but the truth is far beyond his imagination. Neo finds himself targeted by the police when he is contacted by Morpheus, a legendary computer hacker branded a terrorist by the government. Morpheus awakens Neo to the real world, a ravaged wasteland where most of humanity have been captured by a race of machines that live off of the humans' body heat and electrochemical energy and who imprison their minds within an artificial reality known as the Matrix. As a rebel against the machines, Neo must return to the Matrix and confront the agents: super-powerful computer programs devoted to snuffing out Neo and the entire human rebellion.",
                    Year = 1999,
                    ReleaseDate = new DateTime(1999, 03, 31),
                    Runtime = 136
                })
                .Row(new
                {
                    Title = "The Lord of the Rings: The Fellowship of the Ring",
                    Description = "A meek hobbit of the Shire and eight companions set out on a journey to Mount Doom to destroy the One Ring and the dark lord Sauron.",
                    Storyline = "An ancient Ring thought lost for centuries has been found, and through a strange twist in fate has been given to a small Hobbit named Frodo. When Gandalf discovers the Ring is in fact the One Ring of the Dark Lord Sauron, Frodo must make an epic quest to the Cracks of Doom in order to destroy it! However he does not go alone. He is joined by Gandalf, Legolas the elf, Gimli the Dwarf, Aragorn, Boromir and his three Hobbit friends Merry, Pippin and Samwise. Through mountains, snow, darkness, forests, rivers and plains, facing evil and danger at every corner the Fellowship of the Ring must go. Their quest to destroy the One Ring is the only hope for the end of the Dark Lords reign!",
                    Year = 2001,
                    ReleaseDate = new DateTime(2001, 12, 19),
                    Runtime = 178
                });
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}